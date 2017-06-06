using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Common
{
	/// <summary>
	/// 以约定的格式来标识(项目名_主要功能名) 缓存键，便于管理
	/// </summary>
	public class CacheKeys
	{
		//左侧菜单
		public const string IFUTURE_MAIN_MENU = "IFUTURE_MAIN_MENU";
		/// <summary>
		/// 数据中心菜单
		/// </summary>
		public const string IFUTURE_REPORT_MENU = "IFUTURE_REPORT_MENU";
		//系统管理基础数据菜单
		public const string IFUTURE_Sys_MANAGER_MENU = "IFUTURE_Sys_MANAGER_MENU";
		/// <summary>
		/// 头部导航菜单
		/// </summary>
		public const string IFUTURE_HEADER_MENU = "IFUTURE_HEADER_MENU";
		public const string IFUTURE_MHEADER_MENU = "IFUTURE_MHEADER_MENU";

        public const string IFUTURE_M_MENU = "IFUTURE_M_MENU";
        public const string IFUTURE_M_STUMGRMENU = "IFUTURE_M_STUMGRMENU";
		/// <summary>
		/// 背景信息调查设置
		/// </summary>
		public const string IFUTURE_BackInvestigations_MENU = "IFUTURE_BackInvestigations_MENU";
        /// <summary>
        /// 系统所有的url
        /// </summary>
		public const string IFUTURE_SystemMenus_FunctionPath = "IFUTURE_SystemMenus_FunctionPath";
		/// <summary>
		/// 合同完结类型
		/// </summary>
		public const string IFUTURE_CONTRACT_COMPLETETYPE = "IFUTURE_CONTRACT_COMPLETETYPE";
		public const string IFUTURE_FOLDER = "IFUTURE_FOLDER_";

		/// <summary>
		/// Common Essay模板
		/// </summary>
		public const string IFUTURE_COMMONESSAY_TEPLATE = "IFUTURE_COMMONESSAY_TEPLATE";
		//日历模块
		public const string IFUTURE_CALENDAR = "IFUTURE_CALENDAR";
		/// <summary>
		/// 学生分配中与用户相关
		/// </summary>
		public const string IFUTURE_ALLOCATUSER = "IFUTURE_ALLOCATUSER";
		/// <summary>
		/// 学生管理列表中的下拉数据源
		/// </summary>
		public const string IFUTURE_STUDENTLIST_SELECTDATASOURCE = "IFUTURE_STUDENTLIST_SELECTDATASOURCE";
		/// <summary>
		/// 用户具有的功能权限
		/// </summary>
		public const string IFUTURE_USERFUNCTIONS = "IFUTURE_USERFUNCTIONS";
		/// <summary>
		/// 系统当前用户数据
		/// </summary>
		public const string IFUTURE_USERS = "IFUTURE_USERS";
		/// <summary>
		/// 系统当前内部员工老师数据
		/// </summary>
		public const string IFUTURE_TEACHERS = "IFUTURE_TEACHERS";
		/// <summary>
		/// 系统当前学生数据
		/// </summary>
		public const string IFUTURE_STUDENTS = "IFUTURE_STUDENTS";
		/// <summary>
		/// 院校库专业
		/// </summary>
		public const string IFUTURE_MAJORS = "IFUTURE_MAJORS";
		/// <summary>
		/// 菜单权限
		/// </summary>
		public const string IFUTURE_RIGHTMENUDETAIL = "IFUTURE_RIGHTMENUDETAIL_";
		/// <summary>
		/// 系统国家初始数据
		/// </summary>
		public const string IFUTURE_COUNTRIES = "IFUTURE_COUNTRIES";

		/// <summary>
		/// 系统中所有角色数据
		/// </summary>
		public const string IFUTURE_ROLES = "IFUTURE_ROLES";

		/// <summary>
		/// 系统中心和区域下列数据源
		/// </summary>
		public const string IFUTURE_CENTER_AREAS = "IFUTURE_CENTER_AREAS";

		/// <summary>
		/// 系统下拉数据源
		/// </summary>
		public const string IFUTURE_SELECT_DATA = "IFUTURE_SELECT_DATA_";

		/// <summary>
		/// 软实力分类
		/// </summary>
		public const string IFUTURE_SP_CATEGORY = "IFUTURE_SP_CATEGORY_";


		/// <summary>
		/// 软实力城市数据源
		/// </summary>
		public const string IFUTURE_SP_CITYADDRESS = "IFUTURE_SP_CITYADDRESS_";

		/// <summary>
		/// 软实力服务数据
		/// </summary>
		public const string IFUTURE_SP_SOFTPOWERSERVICES = "IFUTURE_SP_SOFTPOWERSERVICES";

		public const string IFUTURE_H5_QUESTION = "IFUTURE_H5_QUESTION";


		public const string IFUTURE_FOLLOW_WAYS = "IFUTURE_FOLLOW_WAYS_";
		/// <summary>
		/// 上传图片尺寸相关设置的缓存Key
		/// </summary>
		public readonly static string ImagesDpiConfigCacheKey = "ImagesDpiConfig_1000";

	}
}
