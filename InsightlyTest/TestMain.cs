using System;

using InsightlySDK;

namespace InsightlyTest{
	class TestMain{
		public static void Main (string[] args){
			string api_key = args[0];
			Insightly i = new Insightly(api_key);
			i.Test();
		}
	}
}
