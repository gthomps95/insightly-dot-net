using System;

using Insightly;

namespace InsightlyTest{
	class TestMain{
		public static void Main (string[] args){
			string api_key = args[0];
			var request = new InsightlyRequest(api_key, "/v2.1/Users");
			var response = request.AsJson();
			Console.WriteLine(response);
		}
	}
}
