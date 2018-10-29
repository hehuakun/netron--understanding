using System;
using System.Runtime.InteropServices;

namespace Netron.Neon {
	
	[ComVisible(true), ComImport(), Guid("64AB4BB7-111E-11d1-8F79-00C04FC2FBE1")]
	public class ShellUIHelper {
		// palceholder
	}

	[
	InterfaceType(ComInterfaceType.InterfaceIsIDispatch),
	ComVisible(true), 
	Guid(@"729FE2F8-1EA8-11d1-8F85-00C04FC2FBE1")
	]
	public interface IShellUIHelper
	{
	    /*
		[hidden, id(DISPID_RESETFIRSTBOOTMODE)] HRESULT ResetFirstBootMode();
		[hidden, id(DISPID_RESETSAFEMODE)] HRESULT ResetSafeMode();
		[hidden, id(DISPID_REFRESHOFFLINEDESKTOP)] HRESULT RefreshOfflineDesktop();
		*/

		/// <summary>
		/// Add a url to the favorites (dialog is displayed).
		/// </summary>
		/// <param name="url">must be a valid Url (including url protocol, etc.)</param>
		/// <param name="title">suggested title for the new entry</param>
		[DispId(4)]
		void AddFavorite([MarshalAs(UnmanagedType.BStr)] string url, string title);
		
		[DispId(5)]
		void AddChannel([MarshalAs(UnmanagedType.BStr)] string url);

		[DispId(6)]
		void AddDesktopComponent([MarshalAs(UnmanagedType.BStr)] string url,
			[MarshalAs(UnmanagedType.BStr)] string type,
            int left, int top, int width,  int height);

		[DispId(7)][return: MarshalAs(UnmanagedType.VariantBool)][PreserveSig]
		bool IsSubscribed([MarshalAs(UnmanagedType.BStr)] string url);

		[DispId(8)]
		void NavigateAndFind([MarshalAs(UnmanagedType.BStr)] string url, 
			[MarshalAs(UnmanagedType.BStr)] string strQuery,
			string varTargetFrame);

		/// <summary>
		/// Handles the importing and exporting of Microsoft® Internet Explorer favorites.
		/// </summary>
		/// <param name="fImport">Bool that specifies one of the following possible values. 
		/// True, if Import is requested. False, if Export is requested.</param>
		/// <param name="strImpExpPath">String, that specifies the location (URL) to import
		///  or export, depending on fImport. If a value is an empty string, a file dialog box is opened.
		///  </param>
		///  <remarks>See also http://msdn.microsoft.com/workshop/browser/external/reference/ifaces/ishelluihelper/importexportfavorites.asp?frame=true</remarks>
		[DispId(9)]
		void ImportExportFavorites([MarshalAs(UnmanagedType.VariantBool)] bool fImport, 
			[MarshalAs(UnmanagedType.BStr)] string strImpExpPath);

		[DispId(10)]
		void AutoCompleteSaveForm([MarshalAs(UnmanagedType.IDispatch)] object form);

		[DispId(11)]
		void AutoScan([MarshalAs(UnmanagedType.BStr)] string strSearch, 
			[MarshalAs(UnmanagedType.BStr)] string strFailureUrl, 
			string pvarTargetFrame);
/*
		[hidden, id(DISPID_AUTOCOMPLETEATTACH)] HRESULT AutoCompleteAttach([optional, in] VARIANT *Reserved);
*/
		/// <summary>
		/// Opens the specified browser dialog box.
		/// </summary>
		/// <param name="bstrName">String that specifies a browser dialog box, using one of the following values. 
		/// 'LanguageDialog' -- Opens the Language Preference dialog box.
		/// 'OrganizeFavorites' -- Opens the Organize NFavorites dialog box.
		/// 'PrivacySettings' -- Microsoft® Internet Explorer 6 and later. Opens the Privacy Preferences dialog box.
		/// 'ProgramAccessAndDefaults' -- Microsoft Windows® XP Service Pack 1 (SP1) and later. Opens the Set Program Access and Defaults dialog box.
		///</param>
		/// <param name="pvarIn">Pointer to a VARIANT that is specific to the UI.</param>
		/// <returns>Pointer to a VARIANT that is specific to the UI</returns>
		/// <remarks>See also http://msdn.microsoft.com/library/default.asp?url=/workshop/browser/external/reference/ifaces/ishelluihelper/showbrowserui.asp</remarks>
		[DispId(13)][return: MarshalAs(UnmanagedType.IUnknown)][PreserveSig]
		object ShowBrowserUI([MarshalAs(UnmanagedType.BStr)] string bstrName, 
			[MarshalAs(UnmanagedType.IUnknown)]  object pvarIn);

	}
}
