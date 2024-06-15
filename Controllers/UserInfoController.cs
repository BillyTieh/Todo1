using PIP_Todo1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PIP_Todo1.Controllers {
    public class UserInfoController : Controller {
        #region 查詢
        /// <summary>
        /// 取得清單員工資料
        /// </summary>
        /// <returns></returns>
        public ActionResult Userlist () {
            TraingEntities db = new TraingEntities();

            List<UserInfo> dbInfo = db.UserInfo.ToList<UserInfo>();

            List<UserInfoViewModel> viewModelList = new List<UserInfoViewModel>();
            foreach(UserInfo info in dbInfo) {
                var tmpInfo = new UserInfoViewModel() {
                    UserID = info.UserID,
                    UserNameEN = info.UserNameEN,
                    Title = info.Title,
                    Email = info.Email,
                    TeamID = info.TeamID,
                    Address = info.Address,
                    TeamName = info.TeamName,
                    CreateDT = info.CreateDT,
                    UpdateDT = info.UpdateDT
                };

                tmpInfo.UserID = info.UserID;

                viewModelList.Add(tmpInfo);
            }           

            return View(viewModelList);
        }
        #endregion

        #region 新增
        public ActionResult Create () {
            return View();
        }

        [HttpPost]
        public ActionResult Create (UserInfoViewModel model) {
            if(ModelState.IsValid) {
                try {
                    using(var db = new TraingEntities()) {
                        var userInfo = new UserInfo {
                            UserID = model.UserID,
                            UserNameEN = model.UserNameEN,
                            Title = model.Title,
                            Email = model.Email,
                            TeamID = model.TeamID,
                            Address = model.Address,
                            TeamName = model.TeamName,
                            CreateDT = DateTime.Now
                        };

                        db.UserInfo.Add(userInfo);
                        db.SaveChanges();

                        // 新增成功後導向到員工資料清單頁面或其他頁面
                        TempData["CreateSuccess"] = "新增資料完成!";
                        return RedirectToAction("Userlist");
                    }
                }
                catch(DbUpdateException ex) {
                    var sqlException = ex.GetBaseException() as SqlException;
                    if(sqlException != null && sqlException.Number == 2601) {
                        // 如果是重複主鍵的錯誤，將 UserID 的錯誤訊息添加到 ModelState 中
                        ModelState.AddModelError("UserID", "UserID 已存在");
                    }
                    else {
                        // 其他錯誤情況
                        ModelState.AddModelError("", "保存資料時出現錯誤");
                    }
                }
                catch(DbEntityValidationException ex) {
                    // 遍歷所有的實體驗證錯誤，並將它們添加到 ModelState 中以顯示給用戶
                    foreach(var validationErrors in ex.EntityValidationErrors) {
                        foreach(var validationError in validationErrors.ValidationErrors) {
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            // 如果資料驗證失敗，返回新增頁面並顯示錯誤訊息
            return View(model);
        }

        //[HttpPost]
        //public ActionResult Create (UserInfoViewModel model) {
        //    if(ModelState.IsValid) {
        //        using(var db = new TraingEntities()) {
        //            var userInfo = new UserInfo {
        //                UserID = model.UserID,
        //                UserNameEN = model.UserNameEN,
        //                Title = model.Title,
        //                Email = model.Email,
        //                TeamID = model.TeamID,
        //                Address = model.Address,
        //                TeamName = model.TeamName,
        //                CreateDT = DateTime.Now
        //            };

        //            db.UserInfo.Add(userInfo);
        //            db.SaveChanges();
        //        }

        //        // 新增成功後導向到員工資料清單頁面或其他頁面
        //        TempData["CreateSuccess"] = "新增資料完成!";
        //        return RedirectToAction("Userlist");
        //    }

        //    // 如果資料驗證失敗，返回新增頁面並顯示錯誤訊息
        //    TempData["CreateError"] = "輸入資料不正確!";
        //    return View(model);
        //}
        #endregion

        #region 編輯
        public ActionResult Edit (string id) {

            TraingEntities db = new TraingEntities();
            UserInfo info = db.UserInfo.FirstOrDefault(u => u.UserID == id);

            UserInfoViewModel viewModel = new UserInfoViewModel {
                UserID = info.UserID,
                UserNameEN = info.UserNameEN,
                Title = info.Title,
                Email = info.Email,
                Address = info.Address,
                TeamID = info.TeamID,
                TeamName = info.TeamName,
                CreateDT = info.CreateDT,
                UpdateDT = info.UpdateDT
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit (UserInfoViewModel viewModel) {
            if(ModelState.IsValid) {
                using(TraingEntities db = new TraingEntities()) {
                    try {
                        // 根據用戶ID檢索用戶信息
                        UserInfo info = db.UserInfo.FirstOrDefault(u => u.UserID == viewModel.UserID);

                        if(info != null) {
                            // 更新用戶信息
                            info.UserNameEN = viewModel.UserNameEN;
                            info.Title = viewModel.Title;
                            info.Email = viewModel.Email;
                            info.Address = viewModel.Address;
                            info.TeamID = viewModel.TeamID;
                            info.TeamName = viewModel.TeamName;
                            info.UpdateDT = DateTime.Now; // 更新更新時間

                            // 保存更改
                            db.SaveChanges();

                            // 設置 TempData，表示是從編輯頁面返回的
                            TempData["FromEditPage"] = true;
                            TempData["UpdateSuccess"] = "資料更新完成!";

                            // 重定向到成功頁面或詳細信息頁面
                            return RedirectToAction("Userlist", "UserInfo");
                        }
                    }
                    catch(DbEntityValidationException ex) {
                        // 遍歷所有的實體驗證錯誤，並將它們添加到ModelState中以顯示給用戶
                        foreach(var validationErrors in ex.EntityValidationErrors) {
                            foreach(var validationError in validationErrors.ValidationErrors) {
                                ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                            }
                        }

                        // 如果保存失敗，顯示錯誤消息
                        ViewBag.ErrorMessage = "輸入資料不正確!";
                    }
                }
            }

            // 如果模型狀態無效，返回編輯視圖以顯示錯誤消息
            return View(viewModel);
        }
        #endregion

        #region 詳細資料
        public ActionResult Details (string id) {

            TraingEntities db = new TraingEntities();
            UserInfo info = db.UserInfo.FirstOrDefault(u => u.UserID == id);

            UserInfoViewModel viewModel = new UserInfoViewModel {
                UserID = info.UserID,
                UserNameEN = info.UserNameEN,
                Title = info.Title,
                Email = info.Email,
                TeamID = info.TeamID,
                Address = info.Address,
                TeamName = info.TeamName,
                CreateDT = info.CreateDT,
                UpdateDT = info.UpdateDT
            };

            return View(viewModel);
        }
        #endregion

        #region 刪除
        public ActionResult Delete (string id) {
            // 根據用戶 ID 檢索用戶信息
            using(TraingEntities db = new TraingEntities()) {
                UserInfo info = db.UserInfo.FirstOrDefault(u => u.UserID == id);

                if(info == null) {
                    return HttpNotFound();
                }

                // 刪除用戶
                db.UserInfo.Remove(info);
                db.SaveChanges();
            }

            // 刪除完成後重定向回用戶列表頁面
            return RedirectToAction("Userlist");
        }
        #endregion
    }
}