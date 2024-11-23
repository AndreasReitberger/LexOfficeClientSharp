using AndreasReitberger.API.LexOffice;
using Newtonsoft.Json;

namespace LexOfficeSharpApi.Test.NUnit
{
    public class Tests
    {
        private const string tokenString = "YOUR_TOKEN";
        private LexOfficeClient? client;

        [SetUp]
        public void Setup()
        {
            client = new LexOfficeClient.LexOfficeConnectionBuilder()
                .WithApiKey(tokenString)
                .Build();
        }

        [Test]
        public void TestJsonSerialization()
        {
            string? json = JsonConvert.SerializeObject(client, Formatting.Indented);
            Assert.That(!string.IsNullOrEmpty(json));

            var client2 = JsonConvert.DeserializeObject<LexOfficeClient>(json);
            Assert.That(client2 is not null);
        }

        [Test]
        public async Task TestWithBuilder()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");

                List<VoucherListContent> invoicesList = await client.GetInvoiceListAsync(LexVoucherStatus.open);
                List<LexQuotation> invoices = await client.GetInvoicesAsync(invoicesList);

                Assert.That(invoices != null && invoices.Count > 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task TestGetInvoices()
        {
            try
            {
                LexOfficeClient handler = new(tokenString);

                List<VoucherListContent> invoicesList = await handler.GetInvoiceListAsync(LexVoucherStatus.open);
                List<LexQuotation> invoices = await handler.GetInvoicesAsync(invoicesList);

                Assert.That(invoices != null && invoices.Count > 0);
            }
            catch (Exception ex) 
            {           
                Assert.Fail(ex.Message);
            }
        }

        /*
        [Test]
        public async Task TestGetQuotations()
        {
            SecureString token = SecureStringHelper.ConvertToSecureString(tokenString);
            LexOfficeSharpApiHandler handler = new LexOfficeSharpApiHandler(token);
            var quotesList = await handler.GetQuotationList(LexVoucherStatus.open);
            var quotes = await handler.GetQuotations(quotesList);

            Assert.IsTrue(quotes != null && quotes.Count > 0);
        }
        
         
        [Test]
        public async Task TestGetContacts()
        {
            SecureString token = SecureStringHelper.ConvertToSecureString(tokenString);
            LexOfficeSharpApiHandler handler = new LexOfficeSharpApiHandler(token);
            var contacts = await handler.GetContacts(LexContactType.Customer);

            Assert.IsTrue(contacts != null && contacts.Count > 0);
        }

        [Test]
        public async Task TestGetContact()
        {
            SecureString token = SecureStringHelper.ConvertToSecureString(tokenString);
            LexOfficeSharpApiHandler handler = new LexOfficeSharpApiHandler(token);
            var contact = await handler.GetContact(new Guid("158c0acb-ac5b-4ecf-ae55-d2ffcb400760"));

            Assert.IsTrue(contact.Id != null);
        }
        */
    }
}