using System;

namespace Insightly{
	public class Insightly{
		public Insightly(String api_key){
			this.api_key = api_key;
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

