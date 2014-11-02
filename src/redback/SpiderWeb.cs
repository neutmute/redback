using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Redback
{
    public delegate void TraversePointCallback(TraverseResult request);

    public class SpiderWeb
    {
        private HashSet<object> _exploredObjects;

        public TraverseSettings Settings { get; set; }

        public SpiderWeb()
        {
            Settings = new TraverseSettings();
            _exploredObjects = new HashSet<object>();
        }

        public void Traverse(Object target, TraversePointCallback callback)
        {
            if (target == null)
            {
                return;
            }

            var request = new TraverseRequest();
            request.Target = new ObjectTarget(target);
            request.Callback = callback;

            var point = new TraverseContext();
            point.Depth = 0;
            Traverse(request, point);
        }

        private void Traverse(TraverseRequest request, TraverseContext context)
        {
            context.Depth++;
            if (request.Target == null)
            {
                return;
            }

            context.RecordExplored(request);

            if (_exploredObjects.Contains(request.Target.Value))
            {

                return;
            }

            if (context.ShouldTerminate(Settings))
            {
                return;
            }

            var hit = GetHit(request, context);
            request.Callback(hit);

            TraverseMembers(request, context);

            context.TargetAsEnumerable = request.Target.Value as IEnumerable;
            if (context.TargetAsEnumerable != null)
            {
                TraverseEnumerable(request, context);
            }

        }

        internal void TraverseMembers(TraverseRequest request, TraverseContext context)
        {
            if (request.Target.Value == null)
            {
                return;
            }
            context.TargetMembers = GetWalkableMembers(request.Target.Type);
            foreach (var memberInfo in context.TargetMembers)
            {
                if (Settings.ExcludeFilters.IsExcluded(memberInfo))
                {
                    continue;
                }

                var invokeRequest = new InvokeRequest();
                invokeRequest.MemberInfo = memberInfo;
                invokeRequest.Target = request.Target;
                invokeRequest.ThrowExceptions = true;

                var invokeResult = InvokeMember(invokeRequest);
                // object mirrorPropertyValue = null;

                //if (_mirrorInUse)
                //{
                //    bool mirrorPropertySwallowedExpection;
                //    mirrorPropertyValue = InvokeMember(targetType, mirrorObject, memberInfo, Options.BindingFlags, true, out mirrorPropertySwallowedExpection);
                //}

                bool isNullPropertyValue = invokeResult.Value == null;
                bool IsTerminating = !isNullPropertyValue && IsTerminatingType(invokeResult.Type);
                //Type propertyValueType = invokeResult.Type;

                if (!IsTerminating)
                {
                    var childTraverseRequest = request.CreateChild(invokeResult, memberInfo);
                    var newPoint = context.Clone();
                    Traverse(childTraverseRequest, newPoint);
                }

                var hit = GetHit(request, context, invokeResult);

                request.Callback(hit);
            }
        }

        private void TraverseEnumerable(TraverseRequest request, TraverseContext context)
        {
            var enumerableTarget = context.TargetAsEnumerable;

            if (enumerableTarget is string)
            {
                request.Callback(GetHit(request, context));
                return;
            }

            var targetAsList = enumerableTarget as IList;

            //MemberInfo countMemberInfo = context.TargetMembers.Find(f => f.Name == "Count");
            //MemberInfo lengthMemberInfo = context.TargetMembers.Find(f => f.Name == "Length");

            //if (countMemberInfo != null)
            //{
            //    var hit = GetHit(request, context);
            //    request.Callback(hit);
            //    //callback(targetType, targetObject, mirrorObject, countField, null, objectName);
            //}

            //if (lengthMemberInfo != null)
            //{
            //    request.Callback(GetHit(request, context));
            //    //callback(targetType, targetObject, mirrorObject, lengthField, null, objectName);
            //}

            if (targetAsList != null)
            {
                for (int i = 0; i < targetAsList.Count; i++)
                {
                    object innerTarget = targetAsList[i];

                    var listPoint = context.Clone();
                    listPoint.IsContainedInEnumerable = true;
                    listPoint.IsContainedInList = true;
                    listPoint.ListIndex = i;

                    var listTarget = new ObjectTarget { Type = innerTarget.GetType(), Value = innerTarget };
                    var listRequest = request.CreateChild(listTarget, null);

                    Traverse(listRequest, listPoint);
                }
            }
            else if (enumerableTarget != null)
            {
                foreach (object innerTarget in enumerableTarget)
                {
                    var enumerablePoint = context.Clone();
                    enumerablePoint.IsContainedInEnumerable = true;
                    enumerablePoint.IsContainedInList = false;
                    enumerablePoint.ListIndex = -1;

                    var listTarget = new ObjectTarget { Type = innerTarget.GetType(), Value = innerTarget };
                    var listRequest = request.CreateChild(listTarget, null);

                    Traverse(listRequest, enumerablePoint);
                }
            }

        }

        #region Private Methods

        private TraverseResult GetHit(TraverseRequest request, TraverseContext context, InvokeResult invokeResult = null)
        {
            var hit = new TraverseResult();
            hit.Parent = request.Parent;
            hit.Owner = request.Target;

            if (hit.Parent != null)
            {
                hit.Parent.IsEnumerable = context.IsContainedInEnumerable;
                hit.Parent.IsListEnumerable = context.IsContainedInList;
                hit.Parent.ListIndex = context.ListIndex;
            }

            if (invokeResult != null)
            {
                hit.Target = invokeResult.ToHitTarget(invokeResult.MemberInfo.Name);
                hit.Target.HitType = Enumerations.Member;
            }
            else
            {
                var hitTarget = new HitTarget();
                hitTarget.HitType = Enumerations.Object;
                hitTarget.Value = request.Target.Value;
                hitTarget.Type = request.Target.Type;
                hitTarget.Name = "Self";
                hit.Target = hitTarget;
            }
            return hit;
        }

        protected InvokeResult InvokeMember(InvokeRequest request)
        {
            var result = new InvokeResult();
            var memberInfo = request.MemberInfo;

            result.MemberInfo = memberInfo;
            var propertyName = memberInfo.Name;
            var targetType = request.Target.Type;
            result.Type = memberInfo.GetPropertyOrFieldType();

            try
            {
                result.Value = targetType.InvokeMember(
                    request.MemberInfo.Name
                    , Settings.BindingFlags
                    , null
                    , request.Target.Value
                    , null);
            }
            catch (TargetInvocationException e)
            {
                string message = string.Format(
                    "Traverse failed to invoke {1}.{0}\r\n\r\nCheck your code isn't throwing the exception or exclude the type.\r\n\r\n"
                    , propertyName
                    , targetType.Name);

                if (request.ThrowExceptions)
                {
                    var ex = new TargetInvocationException(message, e.InnerException);
                    throw ex;
                }

                result.WasExceptionSwallowed = true;
            }
            catch (Exception e2)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// Simple in that it can be asserted directly against. Probably a better way to do this.
        /// </summary>
        protected static bool IsTerminatingType(Type targetType)
        {
            if (targetType == null || targetType == typeof (string) || targetType == typeof (DateTime) ||
                targetType.IsEnum)
            {
                return true;
            }

            bool isSystemNameSpace = targetType.Namespace.StartsWith("System");
            bool isDynamicProxy = targetType.Namespace.Contains("Proxies");
            bool isEnumerable = IsImplementationOf(targetType, typeof (IEnumerable));
            bool isDataTable = targetType == typeof (DataTable);
            bool isDataRow = targetType == typeof (DataRow);

            return isSystemNameSpace && !isDynamicProxy && !isEnumerable && !isDataTable && !isDataRow;
        }

        protected static bool IsImplementationOf(Type targetType, Type interfaceType)
        {
            return targetType.GetInterface(interfaceType.FullName) != null;
        }

        protected static MemberInfoList GetWalkableMembers(Type targetType)
        {
            return GetWalkableMembers(BindingFlags.Public | BindingFlags.Instance, targetType);
        }

        protected static MemberInfoList GetWalkableMembers(BindingFlags bindingFlags, Type targetType)
        {
            var fieldArray = targetType.GetFields(bindingFlags);

            var propertyInfoList = new List<PropertyInfo>();
            propertyInfoList.AddRange(targetType.GetProperties(bindingFlags));

            // ignore indexers
            propertyInfoList.RemoveAll(propertyInfo => propertyInfo.GetIndexParameters().Length > 0);

            var memberInfoList = new MemberInfoList();
            memberInfoList.AddRange(propertyInfoList.ToArray());
            memberInfoList.AddRange(fieldArray);

            return memberInfoList;
        }

        #endregion

    }
}
