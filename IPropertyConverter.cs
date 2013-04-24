using System.Text;
using System.Collections.Generic;
namespace RCPA
{
  public interface IPropertyConverter<T>
  {
    string Version { get; }

    string Name { get; }

    void AddPropertyTo(StringBuilder sb, T t);

    string GetProperty(T t);

    void SetProperty(T t, string value);

    /// <summary>
    /// ����ʵ�����ݣ������������Converter
    /// </summary>
    /// <param name="items">ʵ�������б�</param>
    /// <returns>����Converter�б�Ĭ�Ϸ���null</returns>
    List<IPropertyConverter<T>> GetRelativeConverter(List<T> items);

    /// <summary>
    /// �����ļ���ȡ�����趨��header�������������Converter
    /// </summary>
    /// <param name="header">�ļ���ȡ�������趨�ģ�header</param>
    /// <param name="delimiter">�ļ���ȡ�������趨�ģ��ָ��ַ�</param>
    /// <returns>����Converter�б�Ĭ�Ϸ���null</returns>
    List<IPropertyConverter<T>> GetRelativeConverter(string header, char delimiter);

    bool HasName(string name);

    IPropertyConverter<T> GetConverter(string name);
  }

  public abstract class AbstractPropertyConverter<T> : IPropertyConverter<T>
  {
    #region IPropertyConverter<T> Members

    public virtual string Version
    {
      get { return ""; }
    }

    public abstract string Name { get; }

    public void AddPropertyTo(StringBuilder sb, T t)
    {
      sb.Append(GetProperty(t));
    }

    public abstract string GetProperty(T t);

    public abstract void SetProperty(T t, string value);

    public virtual bool HasName(string name)
    {
      return this.Name.Equals(name);
    }

    public virtual List<IPropertyConverter<T>> GetRelativeConverter(List<T> items)
    {
      return null;
    }

    public virtual List<IPropertyConverter<T>> GetRelativeConverter(string header, char delimiter)
    {
      return null;
    }

    public virtual IPropertyConverter<T> GetConverter(string name)
    {
      return this;
    }

    #endregion

    public override string ToString()
    {
      return this.Name;
    }
  }
}