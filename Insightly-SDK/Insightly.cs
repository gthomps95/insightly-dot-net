using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace InsightlySDK{
	public class Insightly{
		public Insightly(String api_key){
			this.api_key = api_key;
		}
		
		/// <summary>
		/// Get comments for an object.
		/// </summary>
		/// <returns>
		/// Comments associated with object identified by id
		/// </returns>
		/// <param name='id'>
		/// ID of object to get comments of
		/// </param>
		public JArray GetComments(int id){
			return this.Get("/v2.1/Comments/" + id).AsJson<JArray>();
		}
		
		/// <summary>
		/// Delete a comment.
		/// </summary>
		/// <param name='id'>
		/// ID of object to delete
		/// </param>
		public void DeleteComment(int id){
			this.Delete("/v2.1/Comments/" + id).AsString();
		}

		/// <summary>
		/// Creates or updates a comment.
		/// If you are updating an existing comment,
		/// be sure to include the optional comment_id parameter.
		/// </summary>
		/// <returns>
		/// The comment, as returned by the server
		/// </returns>
		/// <param name='body'>
		/// Comment body
		/// </param>
		/// <param name='owner_user_id'>
		/// Owner's user id
		/// </param>
		/// <param name='comment_id'>
		/// Optional comment id (provide this if updating an existing comment)
		/// </param>
		/// <exception cref='ArgumentException'>
		/// Thrown if body is null or zero-length.
		/// </exception>
		public JObject UpdateComment(string body, int owner_user_id, int? comment_id=null){
			if((body == null) || (body.Length < 1)){
				throw new ArgumentException("Comment body cannot be empty.");
			}
			
			JObject data = new JObject();
			data["BODY"] = body;
			data["OWNER_USER_ID"] = owner_user_id;
			if(comment_id != null){
				data["COMMENT_ID"] = comment_id;
			}
			
			return this.Put("/v2.1/Comments").WithBody(data).AsJson<JObject>();
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

