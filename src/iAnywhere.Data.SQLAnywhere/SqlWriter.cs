
// Type: iAnywhere.Data.SQLAnywhere.SqlWriter
// Assembly: iAnywhere.Data.SQLAnywhere.v4.0, Version=11.0.1.27424, Culture=neutral, PublicKeyToken=f222fc4333e0d400
// MVID: CC4F9F8C-E618-49D1-9147-C06A9EF53D1F
// Assembly location: C:\Program Files\SQL Anywhere 11\Assembly\V4\iAnywhere.Data.SQLAnywhere.v4.0.dll

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace iAnywhere.Data.SQLAnywhere
{
  /// <summary>
  /// This extends StringWriter primarily to add the ability to add an indent
  /// to each line that is written out.
  /// </summary>
  internal class SqlWriter : StringWriter
  {
    private int indent = -1;
    private bool atBeginningOfLine = true;

    /// <summary>
    /// The number of tabs to be added at the beginning of each new line.
    /// </summary>
    internal int Indent
    {
      get
      {
        return indent;
      }
      set
      {
                indent = value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    public SqlWriter(StringBuilder b)
      : base(b, CultureInfo.InvariantCulture)
    {
    }

    /// <summary>
    /// Reset atBeginningofLine if we detect the newline string.
    /// <see cref="M:iAnywhere.Data.SQLAnywhere.SqlBuilder.AppendLine" />
    /// Add as many tabs as the value of indent if we are at the
    /// beginning of a line.
    /// </summary>
    /// <param name="value"></param>
    public override void Write(string value)
    {
      if (value == "\r\n")
      {
        base.WriteLine();
                atBeginningOfLine = true;
      }
      else
      {
        if (atBeginningOfLine)
        {
          if (indent > 0)
            base.Write(new string('\t', indent));
                    atBeginningOfLine = false;
        }
        base.Write(value);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void WriteLine()
    {
      base.WriteLine();
            atBeginningOfLine = true;
    }
  }
}
