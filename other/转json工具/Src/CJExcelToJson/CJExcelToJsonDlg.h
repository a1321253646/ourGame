
// CJExcelToJsonDlg.h : ͷ�ļ�
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

// CCJExcelToJsonDlg �Ի���
class CCJExcelToJsonDlg : public CDialogEx
{
// ����
public:
	CCJExcelToJsonDlg(CWnd* pParent = NULL);	// ��׼���캯��

// �Ի�������
	enum { IDD = IDD_CJEXCELTOJSON_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV ֧��


// ʵ��
protected:
	HICON m_hIcon;

	// ���ɵ���Ϣӳ�亯��
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()

public:
	afx_msg void OnBnClickedButtonStart();

public:
	CString m_strFileName;						//��ǰ�򿪵��ļ���
	CString m_arHeader[100];					//��ͷ
	CList<CJsonRow, CJsonRow &> m_lstData;		//����
	int m_nHeaderCount;							//����

	//��ȡexcel�ļ���ע�����excel�ļ����ڱ���������򿪣����޷���ȡ
	BOOL ReadExcel(CString strFileName);
	//����json�ļ�
	void SaveJson();
	//��Unicode�ַ���תΪΪutf8�ַ�����д���ļ���
	void WriteUnicodeStringToUtf8File(FILE *fp, CString strText);
};
