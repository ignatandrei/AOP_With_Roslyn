using Microsoft.CodeAnalysis.CSharp;

namespace AOPRoslyn
{
    /// <summary>
    /// copied from SyntaxKind.PublicKeyword 
    /// </summary>
    public enum ModifierMethod:long
    {
        /// <summary>
        /// no modifier
        /// </summary>
        None= 0,
        /// <summary>
        /// public
        /// </summary>
        PublicKeyword = 1 << 0,
        /// <summary>
        /// private
        /// </summary>
        PrivateKeyword = 1 << 1,
        /// <summary>
        /// internal
        /// </summary>
        InternalKeyword = 1 << 2,
        /// <summary>
        /// protected
        /// </summary>
        ProtectedKeyword = 1 << 3,
        /// <summary>
        /// static
        /// </summary>
        StaticKeyword = 1 << 4,
        /// <summary>
        /// read only
        /// </summary>
        ReadOnlyKeyword = 1 << 5,
        /// <summary>
        /// sealed
        /// </summary>
        SealedKeyword = 1 << 6,        
        /// <summary>
        /// new 
        /// </summary>
        NewKeyword = 1 << 7,
        /// <summary>
        /// override
        /// </summary>
        OverrideKeyword = 1 << 8,
        /// <summary>
        /// abstract
        /// </summary>
        AbstractKeyword = 1 << 9,
        /// <summary>
        /// virtual
        /// </summary>
        VirtualKeyword = 1 << 10,
        /// <summary>
        /// all of above
        /// </summary>
        All = 
            PublicKeyword | PrivateKeyword | InternalKeyword |
            ProtectedKeyword | StaticKeyword | ReadOnlyKeyword |
            SealedKeyword | NewKeyword | OverrideKeyword |
            AbstractKeyword | VirtualKeyword

    }
    /// <summary>
    /// extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// verifies if ModifierMethod has SyntaxKind 
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
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
