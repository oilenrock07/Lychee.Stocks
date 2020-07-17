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
            return "__cfduid=de323b9ac823cdc7f269a305c2e32b2c21564121419; _uc_referrer=https%3A//www.google.com/; _uc_initial_landing_page=https%3A//www.investagrams.com/; _ga=GA1.2.689041548.1564121423; _fbp=fb.1.1564121423362.1863262359; _uc_utm_medium=InvestaPlatform; _uc_utm_campaign=InvestaPrime; _uc_utm_term=; _uc_utm_content=; __tawkuuid=e::investagrams.com::HwBjbyfzeoyTLlvUdRt3JmaFlvO27w/60zDsTwo9mQoDq9lE47f46zld/1IMlWWr::2; _fbc=fb.1.1574068802060.IwAR3CrqMPypr465sGPAkvk4vny_cecYphhjvAcLKHt_RlAJdAJ4vrziquGCw; __gads=ID=8ae7617cb96d5b41:T=1579846771:S=ALNI_MaWEVwnz5BX6FRq7GWVmImL2zRHzg; SL_C_23361dd035530_KEY=5c76797ecab46d505a066c7da7c456a628559ce9; _uc_utm_source=BehavioralTriggerScreener; _gid=GA1.2.1321977028.1594641253; e7bfeae28c72f9a5fae2a6ce78db24e7=684AEA8039BED184A48CFFFD1ED34C7B9B7514DF88090F2A1458E0B1F71B4892FB12A1D0D2903E69FE9F92FD077867288239807F7B119A123DE588541AB46452CA3CD75C61DA788E6E39002E6A434221890D6C1BEDFEE7DA705FAEEF07C105E5A62E1A6D09E30478FED810646250DBC728FB31FBB3A41770E6F9C60B589C691948E14651D7491DC96462C22A67AC739246E551CB10CFBFA4A0DC563C42ADC21DD1798EC6275A5EE055527DF2287A864CA3516CAE633C560AE4133D3EFB6D0C2BF071FBECD89017188B868F2CA0F8A73D4A86263FD56037F5FBA0E151B0B2628A39B450AA8C8A4948750A21B23D0E547D; _uc_last_referrer=direct; _uc_current_session=true; _uc_visits=296; _gat=1";
        }
    }
}
