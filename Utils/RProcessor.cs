﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RCPA.Utils
{
  public class RProcessor : AbstractThreadProcessor
  {
    private string _rExecute;
    private bool _isR;
    private string _rFile;
    private string _expectResultFile;


    public RProcessor(string rExecute, string rFile, string expectResultFile)
    {
      this._rExecute = FileUtils.GetFullLinixName(rExecute);
      var rname = new FileInfo(rExecute).Name.ToLower();
      this._isR = rname.Equals("r.exe") || rname.Equals("r");
      this._rFile = FileUtils.GetFullLinixName(rFile);
      this._expectResultFile = expectResultFile;
    }

    public override IEnumerable<string> Process()
    {
      var rfile = _rFile.ToDoubleQuotes();
      var rproc = new Process
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = _rExecute.ToDoubleQuotes(),
          Arguments = string.Format("--vanilla {0} ", _isR ? "-f " + rfile + " --slave" : rfile),
          UseShellExecute = false,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          CreateNoWindow = true
        }
      };

      try
      {
        if (!rproc.Start())
        {
          throw new Exception("R command cannot be started, check your parameters and ensure that R is available.");
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("R command start failed : {0}", ex.Message));
      }

      try
      {
        string line;
        while ((line = rproc.StandardOutput.ReadLine()) != null)
        {
          Progress.SetMessage(line);
        }

        while ((line = rproc.StandardError.ReadLine()) != null)
        {
          if (line.StartsWith("The system cannot find the path specified"))
          {
            if (Environment.Is64BitOperatingSystem)
            {
              throw new Exception("make sure you setup correct X64 version of the R at Setup->Extenal program!");
            }
            else
            {
              throw new Exception("make sure you setup correct X86 version of the R at Setup->Extenal program!");
            }
          }
          Progress.SetMessage("E:" + line);
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("R command error : {0}", ex.Message));
      }

      if (!string.IsNullOrWhiteSpace(_expectResultFile))
      {
        if (!File.Exists(_expectResultFile))
        {
          throw new Exception(string.Format("R command failed to genearte result as {0}. You can manully run the R script file {1} to find out the problem.", _expectResultFile, _rFile));
        }

        return new string[] { _expectResultFile };
      }
      else
      {
        return null;
      }
    }
  }
}
