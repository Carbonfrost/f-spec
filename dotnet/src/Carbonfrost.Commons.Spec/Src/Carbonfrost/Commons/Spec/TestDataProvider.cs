//
// Copyright 2016, 2017 Carbonfrost Systems, Inc. (http://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Spec.ExecutionModel;

namespace Carbonfrost.Commons.Spec {

    public abstract class TestDataProvider : ITestDataProvider {

        public static ITestDataProvider FromFieldInfo(FieldInfo field) {
            if (field == null) {
                throw new ArgumentNullException("field");
            }
            var accessor = MemberAccessors.Field(field);
            return FromMemberAccessor(accessor);
        }

        public static ITestDataProvider FromPropertyInfo(PropertyInfo property) {
            if (property == null) {
                throw new ArgumentNullException("property");
            }
            var accessor = MemberAccessors.Property(property);
            return FromMemberAccessor(accessor);
        }

        internal static ITestDataProvider FromMemberAccessors(IEnumerable<IMemberAccessor> a) {
            return new CrossJoinMemberAccessorDataProvider(a.ToArray());
        }

        internal static ITestDataProvider FromMemberAccessor(IMemberAccessor a) {
            return new MemberAccessorDataProvider(a);
        }

        public abstract IEnumerable<TestData> GetData(TestContext context);

        class MemberAccessorDataProvider : ITestDataProvider {

            private readonly IMemberAccessor _accessor;

            public MemberAccessorDataProvider(IMemberAccessor accessor) {
                _accessor = accessor;
            }

            IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
                return TestData.Create(context, _accessor);
            }
        }

        class CrossJoinMemberAccessorDataProvider : ITestDataProvider {

            private readonly IMemberAccessor[] _accessors;

            public CrossJoinMemberAccessorDataProvider(IMemberAccessor[] accessors) {
                _accessors = accessors;
            }

            IEnumerable<TestData> ITestDataProvider.GetData(TestContext context) {
                return TestData.Create(context, _accessors);
            }
        }
    }
}
