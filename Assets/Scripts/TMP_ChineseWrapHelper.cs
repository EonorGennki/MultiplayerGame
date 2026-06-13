using TMPro;
using UnityEngine;
using System.Text;

/// <summary>
/// TextMeshPro 中文换行辅助脚本
/// 自动在中文和非中文之间插入零宽空格（\u200B），使长数字/英文能够在中文后正确换行
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class TMP_ChineseWrapHelper : MonoBehaviour
{
    [Header("设置")]
    [Tooltip("是否在运行时自动处理文本")]
    public bool processOnStart = true;

    [Tooltip("是否同时处理数字（0-9）")]
    public bool processDigits = true;

    [Tooltip("是否同时处理英文字母")]
    public bool processLetters = true;

    [Tooltip("原始文本（仅在编辑器中显示用）")]
    [Multiline]
    public string originalText;

    private TMP_Text tmpText;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    void Start()
    {
        if (processOnStart && tmpText != null)
        {
            originalText = tmpText.text;
            tmpText.text = ProcessText(originalText);
        }
    }

    /// <summary>
    /// 处理指定的文本，在中文与非中文之间插入零宽空格
    /// </summary>
    public string ProcessText(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        StringBuilder sb = new StringBuilder(input.Length * 2); // 预分配稍大的容量

        for (int i = 0; i < input.Length; i++)
        {
            char currentChar = input[i];
            sb.Append(currentChar);

            // 不在最后一个字符后处理
            if (i < input.Length - 1)
            {
                char nextChar = input[i + 1];
                bool isCurrentChinese = IsChineseCharacter(currentChar);
                bool isNextChinese = IsChineseCharacter(nextChar);

                bool isCurrentNonChinese = IsNonChineseCharacter(currentChar, processDigits, processLetters);
                bool isNextNonChinese = IsNonChineseCharacter(nextChar, processDigits, processLetters);

                // 情况1：中文 → 非中文（英文/数字）
                if (isCurrentChinese && isNextNonChinese)
                {
                    sb.Append('\u200B'); // 插入零宽空格
                }
                // 情况2：非中文（英文/数字）→ 中文
                else if (isCurrentNonChinese && isNextChinese)
                {
                    sb.Append('\u200B'); // 插入零宽空格
                }
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 判断字符是否为中文字符（基本汉字区：U+4E00 ~ U+9FFF）
    /// 可根据需要扩展其他汉字区
    /// </summary>
    private bool IsChineseCharacter(char c)
    {
        // 基本汉字区（常用汉字）
        if (c >= 0x4E00 && c <= 0x9FFF)
            return true;
        // 扩展A区（不常用，但按需添加）
        // if (c >= 0x3400 && c <= 0x4DBF) return true;
        // 全角标点等不归为中文（如 ，。！？），可按需处理
        return false;
    }

    /// <summary>
    /// 判断字符是否为“非中文但需要处理换行”的字符（英文/数字/部分符号）
    /// </summary>
    private bool IsNonChineseCharacter(char c, bool includeDigits, bool includeLetters)
    {
        // 数字 0-9
        if (includeDigits && c >= '0' && c <= '9')
            return true;

        // 英文字母 A-Z a-z
        if (includeLetters && (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
            return true;

        // 可选：常见的半角符号（如 + - = 等），按需开启
        // 注意：过多符号会导致零宽空格插入过于频繁，影响性能
        // if (c == '+' || c == '-' || c == '=' || c == '/' || c == '*' || c == '_') return true;

        return false;
    }

    /// <summary>
    /// 公共方法：手动刷新文本（用于动态更新文本后调用）
    /// </summary>
    public void RefreshText()
    {
        if (tmpText != null)
        {
            originalText = tmpText.text;
            tmpText.text = ProcessText(originalText);
        }
    }

#if UNITY_EDITOR
    // 编辑器下的辅助方法，方便调试
    [ContextMenu("手动处理当前文本")]
    private void Editor_ProcessText()
    {
        if (tmpText == null)
            tmpText = GetComponent<TMP_Text>();
        originalText = tmpText.text;
        string processed = ProcessText(originalText);
        tmpText.text = processed;
        Debug.Log($"[TMP换行助手] 已处理文本：\n原始：{originalText}\n处理后：{processed}");
    }
#endif
}