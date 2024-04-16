using AndreasReitberger.API.LexOffice;
using AndreasReitberger.Core.Utilities;
using System.Collections.ObjectModel;
using System.Security;

namespace LexOfficeSharpApi.Test.NUnit
{
    public class Tests
    {
        private const string tokenString = "YOUR_TOKEN";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestGetInvoices()
        {
            try
            {
                SecureString token = SecureStringHelper.ConvertToSecureString(tokenString);
                LexOfficeClient handler = new(token);

                ObservableCollection<VoucherListContent> invoicesList = await handler.GetInvoiceListAsync(LexVoucherStatus.open);
                ObservableCollection<LexQuotation> invoices = await handler.GetInvoicesAsync(invoicesList);

                Assert.IsTrue(invoices != null && invoices.Count > 0);
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