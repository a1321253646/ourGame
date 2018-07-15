
// CJExcelToJsonDlg.h : 头文件
//

#pragma once

class CJsonNode
{
public:
	CString Name;
	CString strValue;
	int     nValue;
	CString strType;
};

class CJsonRow
{
public:
	CJsonNode jsonNode[100];
};

// CCJExcelToJsonDlg 对话框
class CCJExcelToJsonDlg : public CDialogEx
{
// 构造
public:
	CCJExcelToJsonDlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
	enum { IDD = IDD_CJEXCELTOJSON_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持


// 实现
protected:
	HICON m_hIcon;

	// 生成的消息映射函数
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()

public:
	afx_msg void OnBnClickedButtonStart();

public:
	CString m_strFileName;						//当前打开的文件名
	CString m_arHeader[100];					//列头
	CList<CJsonRow, CJsonRow &> m_lstData;		//数据
	int m_nHeaderCount;							//列数

	//读取excel文件，注意如果excel文件正在被其它程序打开，则无法读取
	BOOL ReadExcel(CString strFileName);
	//保存json文件
	void SaveJson();
	//将Unicode字符串转为为utf8字符串并写入文件中
	void WriteUnicodeStringToUtf8File(FILE *fp, CString strText);
};
