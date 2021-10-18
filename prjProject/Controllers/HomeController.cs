using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using prjProject.Models;
using System.Web.Security;

namespace prjProject.Controllers
{
    public class HomeController : Controller
    {
        dbFinalProjectEntities db = new dbFinalProjectEntities();
        // GET: Home/Index  首頁
        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/Login  登入
        public ActionResult Login()
        {
            return View();
        }

        // Post: Home/Login  登入
        [HttpPost]
        public ActionResult Login(string UserId, string UserPwd)
        {
            //依帳密取得會員並指定給member
            var member = db.TableCustomers1081728
                        .Where(m => m.UserId == UserId && m.UserPwd == UserPwd)
                        .FirstOrDefault();
            //若member為null 表示未註冊
            if(member == null)
            {
                ViewBag.Message = "帳號或密碼錯誤，登入失敗";
                return View();
            }
            //Session變數記錄歡迎詞
            Session["Welcome"] = member.UserName + "歡迎光臨";

            //Session變數紀錄UserId
            Session["User"] = member.UserId;
            //指定帳號通過登入驗證
            FormsAuthentication.RedirectFromLoginPage(UserId, true);
            return RedirectToAction("Index", "Member");
        }

        // GET: Home/AllPlace  所有景點
        public ActionResult AllPlace(int page = 1) //指定參數預設值為1
        {
            //若 page<1 表示目前 currentPage 在第一頁，否則page及是指定給目前currentPage
            int currentPage = page < 1 ? 1 : page;
            
            //將資料進行排序
            var travel = db.TableTravels1081728.OrderBy(m => m.PlaceId).ToList();
            var result = travel.ToPagedList(currentPage, 4);
            return View(result);
        }

        // GET: Home/Introduction  景點介紹
        public ActionResult Introduction( string CounId = "E01")
        {
            //取得國家名稱
            ViewBag.CountryName = db.TableCountrys1081728
                                  .Where(m => m.CounId == CounId)
                                  .FirstOrDefault().CounName;
            //建立 TravelContent 的 ViewModel物件 tc
            //並指定該物件的 country 屬性值 為所有 TableCountrys1081728 的紀錄
            //指定 place 屬性值為 CounId 參數所對應的 TableTravels1081728 資料表中的所有紀錄
            TravelContent tc = new TravelContent()
            {
                country = db.TableCountrys1081728.ToList(),
                place = db.TableTravels1081728
                .Where(m => m.CounId == CounId).ToList()
            };
            return View(tc);
        }

        // GET: Home/PlaceSearch  景點查詢
        public ActionResult PlaceSearch()
        {
            return View();
        }

        // Post: Home/PlaceSearch  景點查詢
        [HttpPost]
        public ActionResult PlaceSearch(string placename, string country)
        {
            //取得查詢景點
            var place = db.TableTravels1081728
                          .Where(m => m.Place.Contains(placename) && m.CounName == country)
                          .FirstOrDefault();

            //如果 place = null ，表示資料庫中無此景點資料
            if (place == null)
            {
                ViewBag.Message = "查無此景點";
                return View();
            }

            //回傳景點資料
            ViewBag.Country = place.CounName;
            ViewBag.Name = place.Place;
            ViewBag.Content = place.Content;
            ViewBag.Image = place.TraImage;
            return View();
        }


        // GET: Home/Ticket  票券訂購
        public ActionResult Ticket()
        {
            //將資料庫中的票券List出來，並依照票券編號由小到大排序
            var ticket = db.TableTickets1081728
                .OrderByDescending(m => m.TicId)
                .ToList();
            return View(ticket);
        }

        public ActionResult Test()
        {
            return View();
        }
        

        // GET: Home/Register  會員註冊
        public ActionResult Register()
        {
            return View();
        }

        // Post: Home/Register  會員註冊
        [HttpPost]
        public ActionResult Register(TableCustomers1081728 pMember)
        {
            //未通過驗證
            if (ModelState.IsValid == false)
            {
                return View();
            }
            //依帳號取得會員並指定給Member
            var member = db.TableCustomers1081728
                        .Where(m => m.UserId == pMember.UserId)
                        .FirstOrDefault();

            //若member為null 表示會員未註冊
            if (member == null)
            {
                //新增會員資料
                db.TableCustomers1081728.Add(pMember);

                //更新資料庫
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            ViewBag.Message = "此帳號已有人使用，若要註冊請修改帳號";
            return View();
        }

        
    }
}