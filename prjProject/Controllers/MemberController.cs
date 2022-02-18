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
    [Authorize]
    public class MemberController : Controller
    {
        dbFinalProjectEntities db = new dbFinalProjectEntities();

        // GET: Member/Index  首頁
        public ActionResult Index()
        {
            return View();
        }

        // GET: Member/Logout  登出
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Home");
        }

        // GET: Member/Ticket  票券訂購
        public ActionResult Ticket()
        {
            //將資料庫中的票券List出來，並依照票券編號由小到大排序
            var ticket = db.TableTickets1081728
                .OrderByDescending(m => m.TicId)
                .ToList();
            return View(ticket);
        }

        // GET: Member/AllPlace  所有景點
        public ActionResult AllPlace(int page = 1) //指定參數預設值為1
        {
            //若 page<1 表示目前 currentPage 在第一頁，否則page及是指定給目前currentPage
            int currentPage = page < 1 ? 1 : page;

            //將資料進行排序
            var travel = db.TableTravels1081728.OrderBy(m => m.PlaceId).ToList();
            var result = travel.ToPagedList(currentPage, 4);
            return View(result);
        }

        // GET: Member/Introduction  景點介紹
        public ActionResult Introduction(string CounId = "E01")
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

        // GET: Member/PlaceSearch  景點查詢
        public ActionResult PlaceSearch()
        {
            return View();
        }

        // Post: Member/PlaceSearch  景點查詢
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

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult TestResult(int q1, int q2, int q3, int q4, int q5, int q6, int q7)
        {
            int score = q1 + q2 + q3 + q4 + q5 + q6 + q7;
            ViewBag.myScore = score;
            if (score >= 1 && score <= 24)
            {
                ViewBag.Score = "1~24分";
                ViewBag.Type = "悠哉悠哉型";
                ViewBag.Result = "旅行就是要軟爛放空呀～行程那麼滿，整天在趕來趕去，那麼累幹嘛？" +
                    "這類人喜歡悠閒自在的旅行，即使安排好的行程大更動，只要能讓心情愉悅，有什麼不可以？" +
                    "比起喧囂的都市漫遊，更適合放鬆的緩慢的旅遊，像是古蹟之旅、到海島國家度假。";
                ViewBag.Image = "relax.jpeg";
            }
            else if (score >= 25 && score <= 50)
            {
                ViewBag.Score = "25~50分";
                ViewBag.Type = "文化體驗型";
                ViewBag.Result = "對於旅行當地的文化和傳統特色有高度興趣，喜歡透過一些活動接觸不一樣的文化，" +
                    "適合去一些能夠了解人文風情的景點，像是古色古香的九份老街、到日本京都體驗和服、泰國水上市場等，" +
                    "適合預先安排好，做過功課有系統的深度旅行。";
                ViewBag.Image = "culture.jpeg";
            }
            else if (score >= 50)
            {
                ViewBag.Score = "51分以上";
                ViewBag.Type = "滿腔熱血型";
                ViewBag.Result = "旅行對你來說不只是充電，更有環島青年的熱血，會讓你活力滿滿！喜歡時尚快速的旅遊景點，" +
                    "但也喜歡像是泛舟、極限運動這類的活動，即使行程排得比較滿，有點累～還是會熱血地繼續行程，" +
                    "能玩到越多地方越好！可以說走就走，也可以預先安排，但就是要盡情玩啦～";
                ViewBag.Image = "hot.jpeg";
            }
            return View();
        }

        // GET: Member/ShoppingCar  購物車
        public ActionResult ShoppingCar()
        {
            //取得會員登入帳號並指定給 UserId
            string UserId = User.Identity.Name;

            //找出未成為訂單明細的資料，即為購物車內容
            var orderDetails = db.TableCusOrderDetails1081728
                                .Where(m => m.UserId == UserId && m.IsApproved == "否")
                                .ToList();

            //View使用 orderDetails 模型
            return View(orderDetails);
        }

        // Post: Member/ShoppingCar  購物車
        [HttpPost]
        public ActionResult ShoppingCar(string Receiver,string ReEmail,string ReOrderAddress)
        {
            //取得會員登入帳號並指定給 UserId
            string UserId = User.Identity.Name;
            //建立唯一的識別值並指定給guid，當作訂單編號
            string guid = Guid.NewGuid().ToString();

            //建立訂單主檔資料
            TableCusOrders1081728 order = new TableCusOrders1081728();
            order.OrderGuid = guid;
            order.UserId = UserId;
            order.Receiver = Receiver;
            order.ReEmail = ReEmail;
            order.ReOrderAddress = ReOrderAddress;
            order.ReDate = DateTime.Now;
            db.TableCusOrders1081728.Add(order);

            //找出目前在訂單明細中是購物車狀態的票券
            var carList = db.TableCusOrderDetails1081728
                            .Where(m => m.IsApproved == "否" && m.UserId == UserId)
                            .ToList();

            //將購物車狀態票券的 IsApproved 設為 "是"，表示確認訂購產品
            foreach (var item in carList)
            {
                item.OrderGuid = guid;
                item.IsApproved = "是";
            }
            //更新資料庫
            db.SaveChanges();
            return RedirectToAction("OrderList");
        }

        // GET: Member/AddCar  加入購物車
        public ActionResult AddCar(string TicId)
        {
            //取得會員登入帳號並指定給 UserId
            string UserId = User.Identity.Name;

            //IsApproved= "否"表示該票券是購物車狀態
            var currentCar = db.TableCusOrderDetails1081728
                                .Where(m => m.TicId == TicId && m.IsApproved == "否" && m.UserId == UserId)
                                .FirstOrDefault();
            //若currentCar等於null，表示會員選購的票券不是購物車狀態
            if(currentCar == null)
            {
                var ticket = db.TableTickets1081728
                                .Where(m => m.TicId == TicId).FirstOrDefault();
                //將產品放入訂單明細，因為產品的IsApproved為否，表示為購物車狀態
                TableCusOrderDetails1081728 orderDetails = new TableCusOrderDetails1081728();
                orderDetails.UserId = UserId;
                orderDetails.TicId = ticket.TicId;
                orderDetails.TicName = ticket.TicName;
                orderDetails.Price = ticket.Price;
                orderDetails.Qty = 1;
                orderDetails.IsApproved = "否";
                db.TableCusOrderDetails1081728.Add(orderDetails);
            }
            else
            {
                //若票券為購物車狀態，則該產品數量加1
                currentCar.Qty += 1;
            }

            //更新資料庫
            db.SaveChanges();
            return RedirectToAction("ShoppingCar");
        }

        // GET: Member/DeleteCar  刪除購物車
        public ActionResult DeleteCar(int Id)
        {
            //依 Id 找出要刪除購物車狀態的產品
            var orderDetails = db.TableCusOrderDetails1081728
                                .Where(m => m.Id == Id)
                                .FirstOrDefault();
            //刪除購物車狀態的產品
            db.TableCusOrderDetails1081728.Remove(orderDetails);

            //更新資料庫
            db.SaveChanges();
            return RedirectToAction("ShoppingCar");
        }

        // GET: Member/OrderList  我的訂單
        public ActionResult OrderList()
        {
            //取得會員登入帳號並指定給 UserId
            string UserId = User.Identity.Name;

            var orders = db.TableCusOrders1081728
                                .Where(m => m.UserId == UserId)
                                .OrderByDescending(m => m.ReDate).ToList();
            return View(orders);
        }

        // GET: Member/OrderDetail  訂單明細
        public ActionResult OrderDetail(string OrderGuid)
        {
            //根據 OrderGuid 找出和訂單主檔的訂單明細，並指定給orderDetails

            var orderDetails = db.TableCusOrderDetails1081728
                                .Where(m => m.OrderGuid == OrderGuid)
                                .ToList();
            //目前訂單明細的 OrderDetail.cshtml 檢視使用 orderDetails模型
            return View(orderDetails);
        }

        // GET: Member/Edit 編輯會員資料
        public ActionResult Edit()
        {
            //取得目前登入者的 UserId
            var UserId = Session["User"];
            
            //由傳回的 UserId 參數值去尋找欲修改的會員資料
            var customers = db.TableCustomers1081728
                .Where(m => m.UserId == UserId.ToString()).FirstOrDefault();
            
            Session["Welcome"] = customers.UserName;
            //將該會員資料顯示在 Edit.cshtml
            return View(customers);
        }

        // Post: Member/Edit 編輯會員資料
        [HttpPost]
        public ActionResult Edit(TableCustomers1081728 customer)
        {
            if (ModelState.IsValid)
            {
                //在 Edit.cshtml 按下 Submit 會以Post傳回
                //此時再將表單欄位屬性對應到 TableCustomers1081728 的屬性
                //找出欲修改的會員資料並更新

                var temp = db.TableCustomers1081728
                    .Where(m => m.UserId == customer.UserId)
                    .FirstOrDefault();
                //修改資料庫資料
                temp.UserPwd = customer.UserPwd;
                temp.UserName = customer.UserName;
                temp.Gender = customer.Gender;
                temp.Email = customer.Email;
                temp.UserAddress = customer.UserAddress;

                //更新資料
                db.SaveChanges();
                //完成更新後回到首頁
                return RedirectToAction("Index");
            }
            return View(customer);
        }
    }
}