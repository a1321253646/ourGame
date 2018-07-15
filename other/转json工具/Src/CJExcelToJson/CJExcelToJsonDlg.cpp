
// CJExcelToJsonDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "CJExcelToJson.h"
#include "CJExcelToJsonDlg.h"
#include "afxdialogex.h"
#include "include/libxl.h"
#pragma comment (lib, "libxl.lib")
#include <locale>


#ifdef _DEBUG
#define new DEBUG_NEW
#endif

using namespace libxl;
// 用于应用程序“关于”菜单项的 CAboutDlg 对话框

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// 对话框数据
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

// 实现
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CCJExcelToJsonDlg 对话框



CCJExcelToJsonDlg::CCJExcelToJsonDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CCJExcelToJsonDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);

	m_nHeaderCount = 0;
}

void CCJExcelToJsonDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CCJExcelToJsonDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_START, &CCJExcelToJsonDlg::OnBnClickedButtonStart)
END_MESSAGE_MAP()


// CCJExcelToJsonDlg 消息处理程序

BOOL CCJExcelToJsonDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 将“关于...”菜单项添加到系统菜单中。

	// IDM_ABOUTBOX 必须在系统命令范围内。
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// 设置此对话框的图标。  当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO:  在此添加额外的初始化代码

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

void CCJExcelToJsonDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。  对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CCJExcelToJsonDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 用于绘制的设备上下文

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 使图标在工作区矩形中居中
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 绘制图标
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

//当用户拖动最小化窗口时系统调用此函数取得光标
//显示。
HCURSOR CCJExcelToJsonDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CCJExcelToJsonDlg::OnBnClickedButtonStart()
{
	CString	strFilter = L"xlsx文件(*.xlsx)|*.xlsx|xls文件(*.xls)|*.xls|";
	CFileDialog dlg(true, L"", L"", OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, strFilter);

	dlg.m_ofn.lpstrInitialDir = L"c:\\";
	if (IDOK == dlg.DoModal())
	{
		//数据初始化
		m_strFileName = L"";
		for (int i = 0; i < 100; i++)
		{
			m_arHeader[i] = L"";
		}

		m_lstData.RemoveAll();

		//读取excel
		if (ReadExcel(dlg.GetPathName()))
		{
			//保存json
			SaveJson();

			AfxMessageBox(L"转换完毕,json文件保存在excel文件同目录下");
		}
		else
		{
			AfxMessageBox(L"转换失败，请确定excel文件当前没被其它程序打开");
		}
	}
}

//读取excel文件，注意如果excel文件正在被其它程序打开，则无法读取
BOOL CCJExcelToJsonDlg::ReadExcel(CString strFileName)
{
	BOOL isXlsx = false;

	m_strFileName = strFileName;

	strFileName.MakeLower();
	if (strFileName.Find(L".xlsx") >= 0)
	{
		isXlsx = true;
	}

	Book* book;
	
	if (isXlsx)
	{
		book = xlCreateXMLBook();
	}
	else
	{
		book = xlCreateBook();
	}
	

	if (book->load(strFileName)){
		Sheet * sheet = book->getSheet(0);
		if (sheet){
			//读取header
			this->m_nHeaderCount = sheet->lastCol() - sheet->firstCol();

			for (int j = 0; j < this->m_nHeaderCount; j++)
			{
				this->m_arHeader[j] = sheet->readStr(0, j);
			}

			//读取数据
			for (int i = 1; i <= sheet->lastRow(); i++){
				CJsonRow jsonRow;

				for (int j = 0; j < this->m_nHeaderCount; j++)
				{
					jsonRow.jsonNode[j].Name = this->m_arHeader[j];

					CellType celltype = sheet->cellType(i, j);

					if (celltype == CELLTYPE_STRING)
					{
						jsonRow.jsonNode[j].strValue = sheet->readStr(i, j);
						jsonRow.jsonNode[j].strValue.Replace(L"\"", L"\\\"");
						jsonRow.jsonNode[j].strValue.Replace(L"\\N", L"\\\\N");
						jsonRow.jsonNode[j].strValue.Replace(L"\\0", L"");
						jsonRow.jsonNode[j].strValue.Replace(L"\n", L"");
						jsonRow.jsonNode[j].strValue.Replace(L"\r", L"");
						jsonRow.jsonNode[j].strType = L"String";
					}
					else if (celltype == CELLTYPE_NUMBER)
					{
						jsonRow.jsonNode[j].nValue = sheet->readNum(i, j);
						jsonRow.jsonNode[j].strType = L"Number";
					}
					else if (celltype == CELLTYPE_BLANK)
					{
						jsonRow.jsonNode[j].strValue = L"null";
						jsonRow.jsonNode[j].strType = L"String";
					}
					else
					{
						jsonRow.jsonNode[j].strValue = L"null";
						jsonRow.jsonNode[j].strType = L"String";
					}
					
				}

				m_lstData.AddTail(jsonRow);
			}
		}

		return TRUE;
	}
	else{
		return FALSE;
	}
}

int UnicodeToUTF_8(wchar_t *strUnicode, int strUnicodeLen, char *strUTF8, int strUTF8Len)
{
	if ((strUnicode == NULL) || (strUnicodeLen <= 0) || (strUTF8Len <= 0 && strUTF8Len != -1))
	{
		return -1;
	}

	int i, offset = 0;

	if (strUTF8Len == -1)
	{
		for (i = 0; i < strUnicodeLen; i++)
		{
			if (strUnicode[i] <= 0x007f)      //单字节编码  
			{
				offset += 1;
			}
			else if (strUnicode[i] >= 0x0080 && strUnicode[i] <= 0x07ff)   //双字节编码  
			{
				offset += 2;
			}
			else if (strUnicode[i] >= 0x0800 && strUnicode[i] <= 0xffff)   //三字节编码  
			{
				offset += 3;
			}
		}
		return offset + 1;
	}
	else
	{
		if (strUTF8 == NULL)
		{
			return -1;
		}

		for (i = 0; i < strUnicodeLen; i++)
		{
			if (strUnicode[i] <= 0x007f)      //单字节编码  
			{
				strUTF8[offset++] = (char)(strUnicode[i] & 0x007f);
			}
			else if (strUnicode[i] >= 0x0080 && strUnicode[i] <= 0x07ff)   //双字节编码  
			{
				strUTF8[offset++] = (char)(((strUnicode[i] & 0x07c0) >> 6) | 0x00c0);
				strUTF8[offset++] = (char)((strUnicode[i] & 0x003f) | 0x0080);
			}
			else if (strUnicode[i] >= 0x0800 && strUnicode[i] <= 0xffff)   //三字节编码  
			{
				strUTF8[offset++] = (char)(((strUnicode[i] & 0xf000) >> 12) | 0x00e0);
				strUTF8[offset++] = (char)(((strUnicode[i] & 0x0fc0) >> 6) | 0x0080);
				strUTF8[offset++] = (char)((strUnicode[i] & 0x003f) | 0x0080);
			}
		}
		strUTF8[offset] = '\0';
		return offset + 1;
	}
}

//将Unicode字符串转为为utf8字符串并写入文件中
void CCJExcelToJsonDlg::WriteUnicodeStringToUtf8File(FILE *fp, CString strText)
{
	int len = UnicodeToUTF_8(strText.GetBuffer(), strText.GetLength(), NULL, -1);

	if (len == 0)
	{
		return;
	}

	char *pUtf8Text = new char[len];

	UnicodeToUTF_8(strText.GetBuffer(), strText.GetLength(), pUtf8Text, len);

	fwrite(pUtf8Text, sizeof(char), len - 1, fp);
}

//保存json文件
void CCJExcelToJsonDlg::SaveJson()
{
	CString strOutFileName = m_strFileName;

	strOutFileName.Replace(L".xlsx", L".json");
	strOutFileName.Replace(L".xls", L".json");

	FILE *fp;

	if (fopen_s(&fp, (LPCSTR)(_bstr_t)strOutFileName.GetBuffer(), "w"))
	{
		return;
	}

	CString strText;
	int rowIndex = 0;

	strText = L"[";
	WriteUnicodeStringToUtf8File(fp, strText);

	POSITION pos = m_lstData.GetHeadPosition();
	BOOL bFirstRow = true;
	while (pos)
	{
		CJsonRow jsonRow = m_lstData.GetNext(pos);

		//if ((rowIndex > 4680) && (rowIndex < 4685))
		{
			if (!bFirstRow)
			{
				strText = L",";
				WriteUnicodeStringToUtf8File(fp, strText);
			};

			strText = L"{";
			WriteUnicodeStringToUtf8File(fp, strText);

			
			BOOL bFirstColumn = TRUE;
			for (int j = 0; j < m_nHeaderCount; j++)
			//for (int j = 7; j < 8; j++)
			{
				CString strData;

				if (jsonRow.jsonNode[j].strType == L"String")
				{
					strData.Format(L"\"%s\":\"%s\"", m_arHeader[j], jsonRow.jsonNode[j].strValue);
				}
				else if (jsonRow.jsonNode[j].strType == L"Number")
				{
					strData.Format(L"\"%s\":%d", m_arHeader[j], jsonRow.jsonNode[j].nValue);
				}

				if (!bFirstColumn)
				{
					strData = L"," + strData;
				}

				strText = strData;
				WriteUnicodeStringToUtf8File(fp, strText);

				bFirstColumn = FALSE;
			}


			strText = L"}";
			WriteUnicodeStringToUtf8File(fp, strText);
			bFirstRow = FALSE;
		}

		
		rowIndex++;

	}


	strText = L"]";
	WriteUnicodeStringToUtf8File(fp, strText);

	fclose(fp);
}

