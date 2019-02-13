using System.Collections;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationStore.Controllers
{

    public class DataController : Controller
    {
        // GET: api/<controller>
        [Route("api/[controller]")]
        [HttpGet]
        public ArrayList Get()
        {
            /*
            var data = [
                { "url":"http://google.com", "text":"구글"},
                { "url":"http://naver.com", "text":"네이버"},
                { "url":"http://daum.net", "text":"다음"},
                { "url":"http://gdu.co.kr", "text":"구디"}
            ];
            */
            ArrayList data = new ArrayList();
            Hashtable ht = new Hashtable();
            ht.Add("url", "http://google.com");
            ht.Add("text", "구글");
            data.Add(ht);

            ht = new Hashtable();
            ht.Add("url", "http://naver.com");
            ht.Add("text", "네이버");
            data.Add(ht);

            ht = new Hashtable();
            ht.Add("url", "http://daum.net");
            ht.Add("text", "다음");
            data.Add(ht);

            ht = new Hashtable();
            ht.Add("url", "http://gdu.co.kr");
            ht.Add("text", "구디");
            data.Add(ht);

            return data;
        }

    }
}
