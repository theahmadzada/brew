using System.Reflection;

namespace ChaychiMenu.Application;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}