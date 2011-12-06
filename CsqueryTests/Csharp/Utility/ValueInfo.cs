﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using CollectionAssert = NUnit.Framework.CollectionAssert;
using StringAssert = NUnit.Framework.StringAssert;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;
using Jtc.CsQuery;
using Jtc.CsQuery.Utility;
using Jtc.CsQuery.ExtensionMethods;
using Jtc.CsQuery.Utility.StringScanner;

namespace CsqueryTests.Csharp
{
    [TestClass]
    public class ValueInfo_
    {

        [Test,TestMethod]
        public void CharacterInfo_()
        {
            ICharacterInfo charInfo;

            charInfo = CharacterInfo.Create('z');
            Assert.IsTrue(IsOnly(charInfo, "alpha,alphanumeric,lower"), "alpha lower");

            charInfo.Target = 'A';
            Assert.IsTrue(IsOnly(charInfo, "alpha,alphanumeric,upper"), "alpha upper");


            charInfo.Target = ' ';
            Assert.IsTrue(IsOnly(charInfo, "whitespace"), "only whitespace");

            charInfo.Target = '{';
            Assert.IsTrue(IsOnly(charInfo, "bound"), "only bound");

            charInfo.Target = '(';
            Assert.IsTrue(IsOnly(charInfo, "parenthesis,bound"), "only paren & bound");

            charInfo.Target = '[';
            Assert.IsTrue(IsOnly(charInfo, "bound"), "only bound");

            charInfo.Target = '<';
            Assert.IsTrue(IsOnly(charInfo, "bound,operator"), "only operator & bound");

            charInfo.Target = '0';
            Assert.IsTrue(IsOnly(charInfo, "numeric,numericext,alphanumeric"), "numeric");

            charInfo.Target = '9';
            Assert.IsTrue(IsOnly(charInfo, "numeric,numericext,alphanumeric"), "only numeric");

            charInfo.Target = '.';
            Assert.IsTrue(IsOnly(charInfo, "numericext"), "only numeric ext");

            charInfo.Target = '-';
            Assert.IsTrue(IsOnly(charInfo, "numericext,operator"), "only numeric ext & oper");
            
            charInfo.Target = '+';
            Assert.IsTrue(IsOnly(charInfo, "operator"), "only operator");
            
            charInfo.Target = ':';
            Assert.IsTrue(IsOnly(charInfo, ""), "Nothing");

            charInfo.Target = '\\';
            Assert.IsTrue(IsOnly(charInfo, ""), "Nothing");
        }

        [TestMethod]
        public void StringInfo_()
        {
            IStringInfo info;

            info = StringInfo.Create("alllower");
            Assert.IsTrue(IsOnly(info, "alpha,alphanumeric,lower,attribute"), "alpha");

            info.Target = "Mixed";
            Assert.IsTrue(IsOnly(info, "alpha,alphanumeric,attribute"), "only whitespace");
            
            info.Target = "ALLUPPER";
            Assert.IsTrue(IsOnly(info, "alpha,alphanumeric,upper,attribute"), "All upper");

            info.Target = "nn123";
            Assert.IsTrue(IsOnly(info, "alphanumeric,lower,attribute"), "Alphanumeric,attribute");

            info.Target = "    \n";
            Assert.IsTrue(IsOnly(info, "whitespace"), "only whitespace");

            info.Target = "([])";
            Assert.IsTrue(IsOnly(info, "bound"), "only bound");

            info.Target = "()";
            Assert.IsTrue(IsOnly(info, "parenthesis,bound"), "only paren & bound");

            info.Target = "<>";
            Assert.IsTrue(IsOnly(info, "bound,operator"), "only operator & bound");

            info.Target = "0123456789";
            Assert.IsTrue(IsOnly(info, "numeric,numericext,alphanumeric"), "only numeric");


            info.Target = "000";
            Assert.IsTrue(IsOnly(info, "numeric,numericext,alphanumeric"), "only numeric");

            info.Target = "-12.2";
            Assert.IsTrue(IsOnly(info, "numericext"), "only numeric ext");

            info.Target = "+12.2";
            Assert.IsTrue(IsOnly(info, ""), "nothing at all");

            info.Target = "-+";
            Assert.IsTrue(IsOnly(info, "operator"), "only numeric ext & oper");

            info.Target = "data-test";
            Assert.IsTrue(IsOnly(info, "attribute,lower"), "only attribute");

            info.Target = ":data-test";
            Assert.IsTrue(IsOnly(info, "attribute,lower"), "only attribute");

            info.Target = "-data-test";
            Assert.IsTrue(IsOnly(info, "lower"), "invalid attribute");

        }


        protected bool IsOnly(IValueInfo info, string what)
        {
            HashSet<string> testFor = new HashSet<string>(what.Split(','));

            foreach (string val in testFor)
            {
                if (!val.IsOneOf("","attribute","alpha", "alphanumeric", "bound", "lower", "upper", "whitespace", "operator", "parenthesis","numericext","numeric"))
                {
                    throw new Exception("Invalid parm passed to IsOnly");
                }
            }

            bool valid=true;

            valid &= (testFor.Contains("alpha")) ? info.Alpha : !info.Alpha;
            valid &= testFor.Contains("alphanumeric") ? info.Alphanumeric : !info.Alphanumeric;
            valid &= testFor.Contains("numeric") ? info.Numeric : !info.Numeric;
            valid &= testFor.Contains("numericext") ? info.NumericExtended : !info.NumericExtended;
            valid &= testFor.Contains("lower") ? info.Lower : !info.Lower;
            valid &= testFor.Contains("upper") ? info.Upper : !info.Upper;
            valid &= testFor.Contains("whitespace") ? info.Whitespace : !info.Whitespace;
            valid &= testFor.Contains("operator") ? info.Operator : !info.Operator;
            valid &= testFor.Contains("parenthesis") ? info.Parenthesis : !info.Parenthesis;
            if (info is ICharacterInfo)
            {
                valid &= testFor.Contains("quote") ? ((ICharacterInfo)info).Quote : !((ICharacterInfo)info).Quote;
                valid &= testFor.Contains("bound") ? ((ICharacterInfo)info).Bound : !((ICharacterInfo)info).Bound;
            }
            if (info is IStringInfo)
            {
                valid &= testFor.Contains("attribute") ? ((IStringInfo)info).HtmlAttributeName : !((IStringInfo)info).HtmlAttributeName;
            }
            
            return valid;
        }
        
      
    }
}



