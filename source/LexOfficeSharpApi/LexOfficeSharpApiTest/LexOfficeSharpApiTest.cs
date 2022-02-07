using AndreasReitberger.Utilities;
using LexOfficeSharpApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security;
using System.Threading.Tasks;

namespace LexOfficeSharpApiTest
{
    [TestClass]
    public class LexOfficeSharpApiTest
    {
        // https://docs.microsoft.com/en-us/dotnet/core/tutorials/testing-library-with-visual-studio

        private const string tokenString = "1bede9c0-603b-4883-a6ce-9f4676d906c5";
        [TestMethod]
        public async Task TestGetInvoices()
        {
            SecureString token = SecureStringHelper.ConvertToSecureString(tokenString);
            LexOfficeSharpApiHandler handler = new LexOfficeSharpApiHandler(token);

            var invoicesList = await handler.GetInvoiceListAsync(LexVoucherStatus.open);
            var invoices = await handler.GetInvoicesAsync(invoicesList);

            Assert.IsTrue(invoices != null && invoices.Count > 0);
        }


        /*
        [TestMethod]
        public async Task TestGetQuotations()
        {
            SecureString token = SecureStringHelper.ConvertToSecureString(tokenString);
            LexOfficeSharpApiHandler handler = new LexOfficeSharpApiHandler(token);
            var quotesList = await handler.GetQuotationList(LexVoucherStatus.open);
            var quotes = await handler.GetQuotations(quotesList);

            Assert.IsTrue(quotes != null && quotes.Count > 0);
        }
        
         
        [TestMethod]
        public async Task TestGetContacts()
        {
            SecureString token = SecureStringHelper.ConvertToSecureString(tokenString);
            LexOfficeSharpApiHandler handler = new LexOfficeSharpApiHandler(token);
            var contacts = await handler.GetContacts(LexContactType.Customer);

            Assert.IsTrue(contacts != null && contacts.Count > 0);
        }

        [TestMethod]
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
