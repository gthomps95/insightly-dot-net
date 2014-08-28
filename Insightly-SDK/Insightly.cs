using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace InsightlySDK{
	public class Insightly{
		public Insightly(String api_key){
			this.api_key = api_key;
		}
		
		public JArray GetCurrencies(){
			return this.Get("/v2.1/Currencies").AsJson<JArray>();
		}
		
		public JArray GetUsers(){
			return this.Get ("/v2.1/Users/").AsJson<JArray>();
		}
		
		public JObject GetUser(int id){
			return this.Get("/v2.1/Users/" + id).AsJson<JObject>();
		}
		
		public void Test(){
			Console.WriteLine("Testing API .....");
			
			Console.WriteLine("Testing authentication");
			
			int passed = 0;
			int failed = 0;
			
			var currencies = this.GetCurrencies();
			if(currencies.Count > 0){
				Console.WriteLine("Authentication passed...");
				passed += 1;
			}
			else{
				failed += 1;
			}
			
			
			try{
				var users = this.GetUsers();
				JObject user = users[0].Value<JObject>();
				int user_id = user["USER_ID"].Value<int>();
				Console.WriteLine("PASS: GetUsers, found " + users.Count + " users.");
				passed += 1;
			}
			catch(Exception){
				Console.WriteLine("FAIL: GetUsers()");
				failed += 1;
			}
			
			if(failed > 0){
				throw new Exception(failed + " Tests failed!");
			}
			
			Console.WriteLine("All tests passed!");
		}
		
		private InsightlyRequest Get(string url_path){
			return new InsightlyRequest(this.api_key, url_path);
		}
		
		private InsightlyRequest Put(string url_path){
			return (new InsightlyRequest(this.api_key, url_path)).WithMethod(HTTPMethod.PUT);
		}
		
		private InsightlyRequest Post(string url_path){
			return (new InsightlyRequest(this.api_key, url_path)).WithMethod(HTTPMethod.POST);
		}

		private InsightlyRequest Delete(string url_path){
			return (new InsightlyRequest(this.api_key, url_path)).WithMethod(HTTPMethod.DELETE);
		}

		private String api_key;
	}
}

