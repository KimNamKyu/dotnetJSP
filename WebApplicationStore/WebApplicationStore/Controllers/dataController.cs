using System;
using System.Collections;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationStore.Controllers
{

    public class DataController : Controller
    {
        // GET: api/<controller>
        [Route("api/Data")]
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

       
        [Route("api/Search")]
        [HttpGet]
        [EnableCors("AllowOrigin")]
        public Hashtable Search([FromQuery] string name, [FromQuery] string page)
        {
            Hashtable resultMap = new Hashtable();
            int tot = 0;
            ArrayList resultList = new ArrayList();

            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = string.Format("server={0};uid={1};password={2};database={3};","192.168.3.146","root","1234","test");

            try
            {
                conn.Open();
                
                MySqlCommand cmd = null;
                MySqlDataReader sdr = null;
                //총 행수 구하기
                cmd = new MySqlCommand();
                cmd.CommandText = string.Format("select count(*) as tot from list where name like '%{0}%';", name);
                cmd.Connection = conn;
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    tot = Convert.ToInt32(sdr.GetValue(0));
                    Console.WriteLine(tot);
                }
                sdr.Close();


                //리스트 데이터 구하기
                cmd = new MySqlCommand();
                cmd.CommandText = string.Format("select * from list where name like '%{0}%' order by no desc limit {1}, 5;", name, page);
                cmd.Connection = conn;
                sdr =  cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Hashtable ht = new Hashtable();
                    for(int i = 0; i<sdr.FieldCount; i++)
                    {
                        ht.Add(sdr.GetName(i), sdr.GetValue(i));
                    }
                    resultList.Add(ht);
                    
                }
                sdr.Close();
                conn.Close();
            }
            catch 
            {
                Console.WriteLine("connection fail");
            }

            resultMap.Add("top",tot);
            resultMap.Add("rows", resultList);
            return resultMap;
        }
    }
}
