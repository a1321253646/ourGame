
// CJExcelToJson.h : PROJECT_NAME Ӧ�ó������ͷ�ļ�
//

#pragma once

#ifndef __AFXWIN_H__
	#error "�ڰ������ļ�֮ǰ������stdafx.h�������� PCH �ļ�"
#endif

#include "resource.h"		// ������


// CCJExcelToJsonApp: 
// �йش����ʵ�֣������ CJExcelToJson.cpp
//

class CCJExcelToJsonApp : public CWinApp
{
public:
	CCJExcelToJsonApp();

// ��д
public:
	virtual BOOL InitInstance();

// ʵ��

	DECLARE_MESSAGE_MAP()
};

extern CCJExcelToJsonApp theApp;