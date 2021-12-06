using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FixitTicket.Models
{
    public enum Building
    {
        [EnumMember(Value = "John Ballantyne")]
        JohnBallantyne,
        [EnumMember(Value = "Jessie Ballantyne")]
        JessieBallantyne,
        Jackson,
        Werner,
        Price,
        Farley,
        Wemyss,
        Ritter,
        Eiselen,
        Carter,
        Southwest,
        McCaffrey,
        [EnumMember(Value = "Grace Covell")]
        GraceCovell,
        Calaveras,
        Townhouses,
        Monagan,
        Chan,
        [EnumMember(Value = "Theta Chi")]
        ThetaChi,
        [EnumMember(Value = "Alpha Phi")]
        AlphaPhi,
        [EnumMember(Value = "Beta Theta Pi")]
        BetaThetaPi,
        [EnumMember(Value = "Sigma Chi")]
        SigmaChi,
        [EnumMember(Value = "Kappa Alpha Theta")]
        KappaAlphaTheta,
        [EnumMember(Value = "Delta Gamma")]
        DeltaGamma,
        [EnumMember(Value = "Delta Delta Delta")]
        DeltaDeltaDelta
    }
}
