using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Lychee.Stocks.Test
{

    [TestFixture]
    public class WebRequestTest
    {
        [Test]
        public async Task TestConnectingToInvestagrams()
        {
            ////Arrange
            //var service = new HttpClientService();

            ////Act
            ////var response = await service.SendRequest("https://www.investagrams.com/", "/Stock/PSE:JFC", HttpMethod.Get, null);


            ////var response = await service.SendRequest<InvestaViewStockResponse>("https://webapi.investagrams.com/", "/InvestaApi/Stock/viewStock?stockCode=PSE:WEB", HttpMethod.Post, null);
            

            var baseAddress = new Uri("https://webapi.investagrams.com/");
            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var message = new HttpRequestMessage(HttpMethod.Post, "/InvestaApi/Stock/viewStock?stockCode=PSE:WEB");
                message.Headers.Add("cookie", "__cfduid=de323b9ac823cdc7f269a305c2e32b2c21564121419; _uc_referrer=https%3A//www.google.com/; _uc_initial_landing_page=https%3A//www.investagrams.com/; _ga=GA1.2.689041548.1564121423; _fbp=fb.1.1564121423362.1863262359; _uc_utm_medium=InvestaPlatform; _uc_utm_campaign=InvestaPrime; _uc_utm_term=; _uc_utm_content=; _uc_utm_source=PrimeButton; __tawkuuid=e::investagrams.com::HwBjbyfzeoyTLlvUdRt3JmaFlvO27w/60zDsTwo9mQoDq9lE47f46zld/1IMlWWr::2; _fbc=fb.1.1574068802060.IwAR3CrqMPypr465sGPAkvk4vny_cecYphhjvAcLKHt_RlAJdAJ4vrziquGCw; __gads=ID=8ae7617cb96d5b41:T=1579846771:S=ALNI_MaWEVwnz5BX6FRq7GWVmImL2zRHzg; _gid=GA1.2.1133112883.1580945880; e7bfeae28c72f9a5fae2a6ce78db24e7=8286E42EE11CB22E343809E68A4FEB184E36F006AA16DD9FF49FA853CA92E052E51A125E1510A7EBC7BCE1CFD9FEE0756B7E0A0EF44798060A3646AB4C4F73A22D1FADC4C2B31E5D5F3E2A321F848D5BA723802B0A8BD36E8D9E74D5F1D8FFBE22086910768D43ACE51AD99525D4C491B74D5C2F648B375C874DCFF78D84574B742F25EBD27E821FFFD5070F8D29358860F59A359B271CE91DF4B201FCDD37BD2958D9576A53860B35902D80375DFD9E0B470DCA54007C81A357890E766AF575170246CD931A337A56DDF02F63C74489DA9913DC5314B294946844DDC921ACE6A778716C977CA9F71825297031A4759D; _uc_current_session=true; _uc_visits=220; _uc_last_referrer=direct; _gat=1");
                message.Headers.Add("origin", "https://www.investagrams.com");
                var result = await client.SendAsync(message);
                result.EnsureSuccessStatusCode();
            }

        }
    }
}
