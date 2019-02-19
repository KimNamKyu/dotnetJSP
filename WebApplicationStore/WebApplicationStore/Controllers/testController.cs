using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationStore.Controllers
{
    
    public class testController : Controller
    {
        /**********************************
         *  페이징 기법!
         *  조건) 페이지 당 5개 씩 페이지 가 출력됨
         *  1. Get방식일 떄는 화면에 만 보여줌
         *  2. Post 방식일 때는 검색 된 리스트 들이 출력
         **********************************/

        /// <summary>
        /// Get방식으로 리스트 View 출력 Get 방식
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/test")]
        [EnableCors("AllowOrigin")]
        public Hashtable test([FromQuery] string name, [FromQuery] string page)
        {
            Hashtable resultMap = new Hashtable();
            ArrayList list = new ArrayList();
            int tot = 0;
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "Server=192.168.3.146;Database=test;Uid=root;Pwd=1234;";

            try
            {
                conn.Open();
                MySqlCommand cmd = null;
                MySqlDataReader sdr = null;
                //총 행수 구하기
                 cmd = new MySqlCommand();
                cmd.CommandText = string.Format("select count(*) as tot from Files where fName like '%{0}%';", name);
                cmd.Connection = conn;
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    tot = Convert.ToInt32(sdr.GetValue(0));
                }
                sdr.Close();

                //리스트 데이터 구하기
                 cmd = new MySqlCommand();
                cmd.CommandText = string.Format("select No,fNo,fName,url,DATE_FORMAT(regDate,'%Y-%m-%d') as dt from Files where fName like '%{0}%' order by no desc limit {1}, 5;",name,page);
                cmd.Connection = conn;
                 sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Hashtable ht = new Hashtable();
                    for (int i = 0; i < sdr.FieldCount; i++)
                    {
                        ht.Add(sdr.GetName(i), sdr.GetValue(i));
                    }
                    list.Add(ht);
                }
                sdr.Close();
                conn.Close();
            }
            catch
            {
                Console.WriteLine("connection fail");
            }
            resultMap.Add("tot", tot);
            resultMap.Add("rows", list);
            return resultMap;
        }


        /// <summary>
        ///  검색 시 리스트 출력 Post 방식
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/test")]
        [EnableCors("AllowOrigin")]
        public ArrayList test([FromForm] string name)
        {
            ArrayList list = new ArrayList();

            // backend 쪽 (양쪽다 막아 주는 것이 좋다)
            if (name == null)
            {
                return list;
            }
            
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "Server=192.168.3.146;Database=test;Uid=root;Pwd=1234;";

            try
            {
                    conn.Open();
                    Console.WriteLine("open success");
                    MySqlCommand comm = new MySqlCommand();
                comm.CommandText = string.Format("select No,fNo,fName,url,DATE_FORMAT(regDate,'%Y-%m-%d') as dt from Files where fName like '%{0}%'; ",name);
                //comm.CommandText = string.Format("select No,fNo,fName,url,DATE_FORMAT(regDate,'%Y-%m-%d') as dt from Files where fName like '%{0}%' order by no desc limit {1}, 5;", name, page);
                comm.Connection = conn;
                    MySqlDataReader sdr = comm.ExecuteReader();

                    while (sdr.Read())
                    {
                        Hashtable ht = new Hashtable();
                        for (int i = 0; i < sdr.FieldCount; i++)
                        {
                            ht.Add(sdr.GetName(i), sdr.GetValue(i));
                        }
                        list.Add(ht);
                    }
                    sdr.Close();
                    conn.Close();
                }
           
            catch
            {
                Console.WriteLine("fail");
                return null;
            }
            return list;
        }
    }

}
