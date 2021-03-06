using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovinKargar.Core.Utility
{
    public enum DiscountType
    {
        Percentage = 1,
        Amount = 2
    }
    public enum GeoDivisionType
    {
        Country = 0,
        State = 1,
        City = 2,
    }

    public enum StaticContents
    {
        LatestArticleSection = 15,
        firstImageAboutPage = 16,
        WorkingHours = 1008,
        Youtube = 29,
        Instagram = 28,
        Twitter = 27,
        Pinterest = 30,
        Facebook = 26,
        linkedin = 33,
        BlogImage = 1013,
        ContactInfo = 1014,
        companyServices = 3003,
        CopyRight = 3004,
        ImplementaitonService = 3005,
        EmailSubscription = 3,
        
        Address = 5,
        Email = 6,
        Phone = 7,
        ContactUsMap = 4,

        DiscountNews = 5027,

        Logo = 14,
        HeaderImage = 13,
        HomeAboutDescription = 8,

        BlogAd = 32,
    }

    public enum StaticContentTypes
    {
        HeaderFooter = 9,
        About = 13,
        AboutProperties,

        HomeTopSlider = 17,
        HomeAboutSection = 4,
        Contact = 2,

        Guide = 9,
        Popup = 11,
        PageBanner = 12,
        AboutRegisterSteps = 3,
    }

    public enum PaymentStatus
    {
        Unprocessed = 1,
        Failed =2,
        Succeed =3,
        Expired = 4
    }

    public enum AditionalFeatureType
    {
        Volume = 1
    }

}
