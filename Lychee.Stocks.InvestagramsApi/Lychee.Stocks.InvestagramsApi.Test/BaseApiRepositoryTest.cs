using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Services;

namespace Lychee.Stocks.InvestagramsApi.Test
{
    public class BaseApiRepositoryTest
    {
        protected ICookieProviderService _cookieProviderService;

        public BaseApiRepositoryTest()
        {
            _cookieProviderService = new CookieProviderService();
            _cookieProviderService.SetCookie(GetCookie());
        }
        protected string GetCookie()
        {
            return "__cfduid=de323b9ac823cdc7f269a305c2e32b2c21564121419; _uc_referrer=https%3A//www.google.com/; _uc_initial_landing_page=https%3A//www.investagrams.com/; _ga=GA1.2.689041548.1564121423; _fbp=fb.1.1564121423362.1863262359; _uc_utm_medium=InvestaPlatform; _uc_utm_campaign=InvestaPrime; _uc_utm_term=; _uc_utm_content=; __tawkuuid=e::investagrams.com::HwBjbyfzeoyTLlvUdRt3JmaFlvO27w/60zDsTwo9mQoDq9lE47f46zld/1IMlWWr::2; _fbc=fb.1.1574068802060.IwAR3CrqMPypr465sGPAkvk4vny_cecYphhjvAcLKHt_RlAJdAJ4vrziquGCw; __gads=ID=8ae7617cb96d5b41:T=1579846771:S=ALNI_MaWEVwnz5BX6FRq7GWVmImL2zRHzg; SL_C_23361dd035530_KEY=5c76797ecab46d505a066c7da7c456a628559ce9; _uc_utm_source=BehavioralTriggerScreener; _gid=GA1.2.1758613584.1595324368; e7bfeae28c72f9a5fae2a6ce78db24e7=E49D359040DB35F927BFD62C3CF54A36747072193CF3EFB0D5243001E0183D12D5F067F13FB0A604D4718D16C33390945A3DA805DEF28F6E089040897BB5E050B423B9CFB6530E6C0E1E58BFC1246F3864826746858D40F3E5DE4357DD5A9081E3281537052F5AA089FD9FCBAD172BDCC3B68518EBB6D7297795C7F0BB59B8ED3E21FBBBAED891447898F5C4C638034E69DF8C9F5CA9EE4D97B3204ABA3AF086E1A7BB3F3D886A57BCA909369F911146D7CA121DADAC90F399BFB00BDC2F6DBF9ACF6AD0E9C8FABCC3CFAEF1026217B940C81BE71C6A3A9D29E9A23826B580738522E53E0B2B39ED02802E09728C2516; _uc_visits=310; _uc_last_referrer=http%3A//stocks.localhost/; _gat=1";
        }
    }
}
