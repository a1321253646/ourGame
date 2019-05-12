#if UNITY_EDITOR
using UnityEngine;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Xml;

namespace Together {
	
	public static class TGPostProcess {

		internal static bool CopyAndReplaceDirectory(string srcPath, string dstPath) {
            if (Directory.Exists(srcPath)) {
				if (Directory.Exists(dstPath))
					Directory.Delete(dstPath, true);
				if (File.Exists(dstPath))
					File.Delete(dstPath);
				Directory.CreateDirectory(dstPath);
			    foreach (var file in Directory.GetFiles(srcPath))
				    File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));
			    foreach (var dir in Directory.GetDirectories(srcPath))
				    CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
				return true;
            }
			return false;
		}

		internal static bool CopyAndReplaceFile(string srcPath, string dstPath, string filePath) {
            if (File.Exists (srcPath)) {
				if (File.Exists(dstPath+filePath))
					File.Delete(dstPath+filePath);
				Directory.CreateDirectory(dstPath);
				File.Copy (srcPath, dstPath + filePath);
				return true;
			}
			return false;
		}

	    [PostProcessBuild()]
	    public static void OnPostProcessBuild( BuildTarget buildTarget, string pathToBuiltProject ) {
#if UNITY_IOS
#if UNITY_5 || UNITY_2017 || UNITY_2018
		    if (buildTarget == BuildTarget.iOS) {
#else
			if (buildTarget == BuildTarget.iPhone) {
#endif
			    string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
			    PBXProject proj = new PBXProject();
			    proj.ReadFromString(File.ReadAllText(projPath));
			    string target = proj.TargetGuidByName("Unity-iPhone");

				string basePatch = "TGSDK/Plugins/1.8.3/iOS/";
				string sdkPath = pathToBuiltProject+"/Frameworks/"+basePatch;
				string libPath = pathToBuiltProject+"/Libraries/"+basePatch;

				proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", sdkPath+"frameworks/");
				proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", libPath);

			    List<string> frameworkFiles = new List<string>();

				frameworkFiles.Add("frameworks/AdColony.framework");
                frameworkFiles.Add("frameworks/AppLovinSDK.framework");
                frameworkFiles.Add("frameworks/BaiduMobAdSDK.framework");
                frameworkFiles.Add("frameworks/CHAMoatMobileAppKit.framework");
                frameworkFiles.Add("frameworks/Chartboost.framework");
                frameworkFiles.Add("frameworks/FBAudienceNetwork.framework");
                frameworkFiles.Add("frameworks/FMDB.framework");
                frameworkFiles.Add("frameworks/GDTMobSDK.framework");
                frameworkFiles.Add("frameworks/GoogleMobileAds.framework");
                frameworkFiles.Add("frameworks/InMobi.framework");
                frameworkFiles.Add("frameworks/IronSource.framework");
                frameworkFiles.Add("frameworks/MTGSDK.framework");
                frameworkFiles.Add("frameworks/MTGSDKInterstitial.framework");
                frameworkFiles.Add("frameworks/MTGSDKInterstitialVideo.framework");
                frameworkFiles.Add("frameworks/MTGSDKReward.framework");
                frameworkFiles.Add("frameworks/SQMobileAdsSDK.framework");
                frameworkFiles.Add("frameworks/TGSDK.framework");
                frameworkFiles.Add("frameworks/Tapjoy.framework");
                frameworkFiles.Add("frameworks/UnityAds.framework");
                frameworkFiles.Add("frameworks/VungleSDK.framework");
                frameworkFiles.Add("frameworks/WMAdSDK.framework");
                frameworkFiles.Add("frameworks/WindSDK.framework");
                frameworkFiles.Add("frameworks/yomobads.framework");

				foreach (string file in frameworkFiles) {
					bool frameworkExists = CopyAndReplaceDirectory("Assets/"+basePatch+file, sdkPath+file);
					if (frameworkExists) {
						string fileGuid  = proj.AddFile(sdkPath+file,"Frameworks/"+basePatch+file,PBXSourceTree.Source);
						proj.AddFileToBuild(target, fileGuid);
					}
				}

				List<string> bundleFiles = new List<string>();

				bundleFiles.Add("resources/SQMobileAdsSDK.bundle");
                bundleFiles.Add("resources/TGADSDK.bundle");
                bundleFiles.Add("resources/TapjoyResources.bundle");
                bundleFiles.Add("resources/WMAdSDK.bundle");
                bundleFiles.Add("resources/baidumobadsdk.bundle");

				foreach (string file in bundleFiles) {
					bool bundleExists = CopyAndReplaceDirectory("Assets/"+basePatch+file, sdkPath+file);
					if (bundleExists) {
						string fileGuid  = proj.AddFile(sdkPath+file,"Frameworks/"+basePatch+file,PBXSourceTree.Source);
						proj.AddFileToBuild(target, fileGuid);
					}
				}

				List<string> singleFiles = new List<string>();

				singleFiles.Add("resources/TGStartedIcon.png");
                singleFiles.Add("resources/TGUnstartedIcon.png");

				foreach (string file in singleFiles) {
					bool singleExists = CopyAndReplaceFile("Assets/"+basePatch+file, sdkPath, file);
					if (singleExists) {
						string fileGuid  = proj.AddFile(sdkPath+file,"Frameworks/"+basePatch+file,PBXSourceTree.Source);
						proj.AddFileToBuild(target, fileGuid);
					}
				}
					
				proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");

				proj.AddFrameworkToProject(target,"Accelerate.framework",true);
				proj.AddFrameworkToProject(target,"AdSupport.framework",true);
				proj.AddFrameworkToProject(target,"AudioToolbox.framework",true);
				proj.AddFrameworkToProject(target,"AVFoundation.framework",true);
				proj.AddFrameworkToProject(target,"CFNetwork.framework",true);
				proj.AddFrameworkToProject(target,"CoreGraphics.framework",true);
				proj.AddFrameworkToProject(target,"CoreLocation.framework",true);
				proj.AddFrameworkToProject(target,"CoreMedia.framework",true);
				proj.AddFrameworkToProject(target,"CoreMotion.framework",true);
				proj.AddFrameworkToProject(target,"CoreTelephony.framework",true);
				proj.AddFrameworkToProject(target,"CoreVideo.framework",true);
				proj.AddFrameworkToProject(target,"EventKit.framework",true);
				proj.AddFrameworkToProject(target,"Foundation.framework",true);
				proj.AddFrameworkToProject(target,"GLKit.framework",true);
				proj.AddFrameworkToProject(target,"iAd.framework",true);
				proj.AddFrameworkToProject(target,"ImageIO.framework",true);
				proj.AddFrameworkToProject(target,"JavaScriptCore.framework",true);
				proj.AddFrameworkToProject(target,"MediaPlayer.framework",true);
				proj.AddFrameworkToProject(target,"MessageUI.framework",true);
				proj.AddFrameworkToProject(target,"MobileCoreServices.framework",true);
				proj.AddFrameworkToProject(target,"QuartzCore.framework",true);
				proj.AddFrameworkToProject(target,"SafariServices.framework",true);
				proj.AddFrameworkToProject(target,"Security.framework",true);
				proj.AddFrameworkToProject(target,"Social.framework",true);
				proj.AddFrameworkToProject(target,"StoreKit.framework",true);
				proj.AddFrameworkToProject(target,"SystemConfiguration.framework",true);
				proj.AddFrameworkToProject(target,"UIKit.framework",true);
                proj.AddFrameworkToProject(target,"WatchConnectivity.framework",true);
				proj.AddFrameworkToProject(target,"WebKit.framework",true);

				proj.AddFileToBuild (target, proj.AddFile ("usr/lib/libsqlite3.tbd", "Frameworks/libsqlite3.tbd", PBXSourceTree.Sdk));
				proj.AddFileToBuild (target, proj.AddFile ("usr/lib/libxml2.tbd", "Frameworks/libxml2.tbd", PBXSourceTree.Sdk));
				proj.AddFileToBuild (target, proj.AddFile ("usr/lib/libz.tbd", "Frameworks/libz.tbd", PBXSourceTree.Sdk));
				proj.AddFileToBuild (target, proj.AddFile ("usr/lib/libc++.tbd", "Frameworks/libc++.tbd", PBXSourceTree.Sdk));
//				proj.AddFileToBuild (target, proj.AddFile ("usr/lib/libstdc++.tbd", "Frameworks/libstdc++.tbd", PBXSourceTree.Sdk));

				File.WriteAllText(projPath, proj.WriteToString());

				UpdateIOSPlist (pathToBuiltProject);
			}
#endif
		}

		private static void UpdateIOSPlist(string path) {
#if UNITY_IOS

			string plistPath = Path.Combine(path, "Info.plist");   

			PlistDocument plist = new PlistDocument();
			plist.ReadFromString (File.ReadAllText (plistPath));


			//Get Root
			PlistElementDict rootDict = plist.root;


			PlistElementString calendarPrivacy = (PlistElementString)rootDict["NSCalendarsUsageDescription"];
			if (calendarPrivacy == null) {	
				rootDict.SetString ("NSCalendarsUsageDescription","Some ad content may create a calendar event.");
			}

			PlistElementString photoPrivacy = (PlistElementString)rootDict["NSPhotoLibraryUsageDescription"];
			if (photoPrivacy == null) {	
				rootDict.SetString ("NSPhotoLibraryUsageDescription","Some ad content may require access to the photo library.");
			}

			PlistElementString cameraPrivacy = (PlistElementString)rootDict["NSCameraUsageDescription"];
			if (cameraPrivacy == null) {	
				rootDict.SetString ("NSCameraUsageDescription","Some ad content may access camera to take picture.");
			}

			PlistElementString motionPrivacy = (PlistElementString)rootDict["NSMotionUsageDescription"];
			if (motionPrivacy == null) {	
				rootDict.SetString ("NSMotionUsageDescription","Some ad content may require access to accelerometer for interactive ad experience.");
			}
			 
			File.WriteAllText (plistPath, plist.WriteToString ());
#endif

	     }
	
     }

}
#endif
