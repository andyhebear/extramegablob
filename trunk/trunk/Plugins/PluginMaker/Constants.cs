using System;

namespace PluginMaker
{
    static class Constants
    {
        private static String AssemblyCopyright
        {
            get
            {
                object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((System.Reflection.AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }
        private static DateTime DateCompiled()
        {
            System.Version v = version;
            DateTime d = new DateTime(
                v.Build * TimeSpan.TicksPerDay +
                v.Revision * TimeSpan.TicksPerSecond * 2
                ).AddYears(1999).AddHours(1);
            return d.Subtract(new TimeSpan(24, 0, 0));
        }
        public static Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        public static string hr = "____________________________________________________________";
        public static string header = @"
PluginMaker v" + version + @" Compiled " + DateCompiled().ToString() + @"
" + AssemblyCopyright + @"
" + hr + @"
";
        public const string help = @"

This program is to generate new plugins, it writes the VS project file and the main .cs file (a future version will support more languages)
Options: 
s Supress Console Output
w Wait for key press before exiting
v Verbose logging
o Overwrite output files if they exist (caution)

usage: 
PluginMaker <Options> <PluginName>

<PluginName> - The name of the new plugin to be made.

Example:
PluginMaker ""vw"" ""memeZone""

";
        public static string[] errors = {
                                            "",
                                            "Invalid number of arguments",
                                            "Exception Encountered",
                                            "A file already exists, try option o",
                                            "output plugin/file name/path is too long"};


        public static string template_proj_cs = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProductVersion>8.0.30703</ProductVersion>
    <SchemaVersion>2.0</SchemaVersion>
    <ProjectGuid><{TOKEN2}></ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace><{TOKEN1}></RootNamespace>
    <AssemblyName><{TOKEN1}></AssemblyName>
    <TargetFrameworkVersion>v4.0</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>..\bin\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)' == 'Debug|x64'"">
    <DebugSymbols>true</DebugSymbols>
    <OutputPath>..\bin\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <DebugType>full</DebugType>
    <PlatformTarget>x64</PlatformTarget>
    <CodeAnalysisLogFile>bin\Debug\<{TOKEN1}>.dll.CodeAnalysisLog.xml</CodeAnalysisLogFile>
    <CodeAnalysisUseTypeNameInSuppression>true</CodeAnalysisUseTypeNameInSuppression>
    <CodeAnalysisModuleSuppressionsFile>GlobalSuppressions.cs</CodeAnalysisModuleSuppressionsFile>
    <ErrorReport>prompt</ErrorReport>
    <CodeAnalysisRuleSet>MinimumRecommendedRules.ruleset</CodeAnalysisRuleSet>
    <CodeAnalysisRuleSetDirectories>;C:\Program Files (x86)\Microsoft Visual Studio 10.0\Team Tools\Static Analysis Tools\\Rule Sets</CodeAnalysisRuleSetDirectories>
    <CodeAnalysisRuleDirectories>;C:\Program Files (x86)\Microsoft Visual Studio 10.0\Team Tools\Static Analysis Tools\FxCop\\Rules</CodeAnalysisRuleDirectories>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)' == 'Release|x64'"">
    <OutputPath>bin\x64\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <Optimize>true</Optimize>
    <DebugType>pdbonly</DebugType>
    <PlatformTarget>x64</PlatformTarget>
    <CodeAnalysisLogFile>bin\Release\<{TOKEN1}>.dll.CodeAnalysisLog.xml</CodeAnalysisLogFile>
    <CodeAnalysisUseTypeNameInSuppression>true</CodeAnalysisUseTypeNameInSuppression>
    <CodeAnalysisModuleSuppressionsFile>GlobalSuppressions.cs</CodeAnalysisModuleSuppressionsFile>
    <ErrorReport>prompt</ErrorReport>
    <CodeAnalysisRuleSet>MinimumRecommendedRules.ruleset</CodeAnalysisRuleSet>
    <CodeAnalysisRuleSetDirectories>;C:\Program Files (x86)\Microsoft Visual Studio 10.0\Team Tools\Static Analysis Tools\\Rule Sets</CodeAnalysisRuleSetDirectories>
    <CodeAnalysisRuleDirectories>;C:\Program Files (x86)\Microsoft Visual Studio 10.0\Team Tools\Static Analysis Tools\FxCop\\Rules</CodeAnalysisRuleDirectories>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)' == 'Debug|x86'"">
    <DebugSymbols>true</DebugSymbols>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
    <DebugType>full</DebugType>
    <PlatformTarget>x86</PlatformTarget>
    <ErrorReport>prompt</ErrorReport>
    <CodeAnalysisIgnoreBuiltInRuleSets>false</CodeAnalysisIgnoreBuiltInRuleSets>
    <CodeAnalysisIgnoreBuiltInRules>false</CodeAnalysisIgnoreBuiltInRules>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)' == 'Release|x86'"">
    <OutputPath>bin\x86\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <Optimize>true</Optimize>
    <DebugType>pdbonly</DebugType>
    <PlatformTarget>x86</PlatformTarget>
    <ErrorReport>prompt</ErrorReport>
    <CodeAnalysisIgnoreBuiltInRuleSets>true</CodeAnalysisIgnoreBuiltInRuleSets>
    <CodeAnalysisIgnoreBuiltInRules>false</CodeAnalysisIgnoreBuiltInRules>
    <CodeAnalysisFailOnMissingRules>true</CodeAnalysisFailOnMissingRules>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""ExtraMegaBlob.References"">
      <HintPath>..\..\bin\ExtraMegaBlob.References.dll</HintPath>
    </Reference>
    <Reference Include=""eyecm.PhysX"">
      <HintPath>..\..\bin\eyecm.PhysX.dll</HintPath>
    </Reference>
    <Reference Include=""Mogre"">
      <HintPath>..\..\bin\Mogre.dll</HintPath>
    </Reference>
    <Reference Include=""MogreFramework, Version=0.1.3987.42137, Culture=neutral, processorArchitecture=AMD64"">
      <SpecificVersion>False</SpecificVersion>
      <HintPath>..\..\bin\MogreFramework.dll</HintPath>
    </Reference>
    <Reference Include=""MOIS"">
      <HintPath>..\..\bin\MOIS.dll</HintPath>
    </Reference>
    <Reference Include=""System"" />
    <Reference Include=""System.Windows.Forms"" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include=""<{TOKEN1}>.cs"" />
  </ItemGroup>
  <ItemGroup>
    <Folder Include=""Properties\"" />
  </ItemGroup>
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
  <PropertyGroup>
    <PostBuildEvent>mkdir ""$(SolutionDir)\..\bin\servercache\<{TOKEN1}>\""
mkdir ""$(SolutionDir)\..\bin\cache\<{TOKEN1}>\""
copy ""$(ProjectDir)\<{TOKEN1}>.cs"" ""$(SolutionDir)\..\bin\servercache\<{TOKEN1}>\<{TOKEN1}>.cs""
copy ""$(ProjectDir)\<{TOKEN1}>.cs"" ""$(SolutionDir)\..\bin\cache\<{TOKEN1}>\<{TOKEN1}>.cs""</PostBuildEvent>
  </PropertyGroup>
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name=""BeforeBuild"">
  </Target>
  <Target Name=""AfterBuild"">
  </Target>
  -->
</Project>";
        public static string template_code_cs = @"using System;
using System.Collections.Generic;
using System.Text;
using MogreFramework;
using ExtraMegaBlob.References;
using Mogre;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;
using MOIS;
using Mogre.PhysX;
namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ClientPlugin
    {
        public override Hashtable meshes_lookup()
        {
            Hashtable h = new Hashtable();
            #region meshes
            #endregion
            return h;
        }
        public override Hashtable skeletons_lookup()
        {
            Hashtable h = new Hashtable();
            #region meshes
            #endregion
            return h;
        }
        public override Hashtable materials_lookup()
        {
            Hashtable h = new Hashtable();
            #region materials
            #endregion
            return h;
        }
        public override void init()
        {
            log(""init"");
        }
        public override void shutdown()
        {
            ready = false;
            log(""shutting down!"");
            log(""done shutting down!"");
        }
        public override ExtraMegaBlob.References.Vector3 Location()
        {
            return new ExtraMegaBlob.References.Vector3(0f, 0f, 0f);
        }
        public override string Name()
        {
            return ""<{TOKEN3}>"";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] { };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { };
        }
        public override void inbox(ExtraMegaBlob.References.Event ev)
        {
            if (!ready) return;
            switch (ev._Keyword)
            {
                default:
                    break;
            }
        }
        public override void updateHook()
        {
            if (ready)
            {
            }
        }
        private bool ready = false;
        public override void frameHook(float interpolation)
        {
        }
    }
}";
    }
}
