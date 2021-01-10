using AOPMethodsCommon;

namespace AOPMethodsTest
{
    [AutoEnum(template = EnumMethod.GenerateExtensionCode)]
    /// <summary>
    /// my test
    /// </summary>
    public enum Test:long
    {
        a,
        //the b should be 1
        b,
        /// <summary>
        /// x should be 2
        /// </summary>
        x=5,
        y=7

    }
    [AutoEnum(template = EnumMethod.GenerateExtensionCode)]
    public enum Test2 
    {
        a1,
        b1,
        /// <summary>
        /// x should be 2
        /// </summary>
        x1 = 5,
        y1 = 7

    }
}
