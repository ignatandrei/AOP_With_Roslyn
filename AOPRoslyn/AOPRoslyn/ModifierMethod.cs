using Microsoft.CodeAnalysis.CSharp;

namespace AOPRoslyn
{
    /// <summary>
    /// copied from SyntaxKind.PublicKeyword 
    /// </summary>
    public enum ModifierMethod:long
    {
        None= 0,
        PublicKeyword = 1 << 0,
        PrivateKeyword = 1 << 1,
        InternalKeyword = 1 << 2,
        ProtectedKeyword = 1 << 3,
        StaticKeyword = 1 << 4,
        ReadOnlyKeyword = 1 << 5,
        SealedKeyword = 1 << 6,        
        NewKeyword = 1 << 7,
        OverrideKeyword = 1 << 8,
        AbstractKeyword = 1 << 9,
        VirtualKeyword = 1 << 10,
        All = 
            PublicKeyword | PrivateKeyword | InternalKeyword |
            ProtectedKeyword | StaticKeyword | ReadOnlyKeyword |
            SealedKeyword | NewKeyword | OverrideKeyword |
            AbstractKeyword | VirtualKeyword

    }

    public static class Extensions
    {
        public static bool IsOnEnum(this ModifierMethod modifier,SyntaxKind sn)
        {
            var mm = ModifierMethod.None;
            switch (sn)
            {
                case SyntaxKind.PublicKeyword:
                    mm = ModifierMethod.PublicKeyword;
                    break;
                case SyntaxKind.PrivateKeyword       :
                    mm = ModifierMethod.PrivateKeyword;
                    break;
                case SyntaxKind.InternalKeyword      :
                    mm = ModifierMethod.InternalKeyword;
                    break;
                case SyntaxKind.ProtectedKeyword     :
                    mm = ModifierMethod.ProtectedKeyword;
                    break;
                case SyntaxKind.StaticKeyword        :
                    mm = ModifierMethod.StaticKeyword;
                    break;
                case SyntaxKind.ReadOnlyKeyword:
                    mm = ModifierMethod.ReadOnlyKeyword;
                    break;
                case SyntaxKind.SealedKeyword        :
                    mm = ModifierMethod.SealedKeyword;
                    break;
                case SyntaxKind.NewKeyword           :
                    mm = ModifierMethod.NewKeyword;
                    break;
                case SyntaxKind.OverrideKeyword      :
                    mm = ModifierMethod.OverrideKeyword;
                    break;
                case SyntaxKind.AbstractKeyword      :
                    mm = ModifierMethod.AbstractKeyword;
                    break;
                case SyntaxKind.VirtualKeyword:
                    mm = ModifierMethod.VirtualKeyword;
                    break;

            }
            return ((modifier & mm) == mm);
            
        }
    }
}
