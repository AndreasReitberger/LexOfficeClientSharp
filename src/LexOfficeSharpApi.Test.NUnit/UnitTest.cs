using AndreasReitberger.API.LexOffice;
using AndreasReitberger.API.LexOffice.Enum;
using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using System.Security;

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
        public async Task TestGetInvoicesOpen()
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

        [Test]
        public async Task TestCreateInvoices()
        {
            try
            {
                SecureString token = SecureStringHelper.ConvertToSecureString(tokenString);
                LexOfficeClient handler = new(token);

                // Create a new invoice object
                var invoice = new LexCreateInvoice()
                {
                    Address = new LexContactAddress()
                    {
                        Name = "Bike & Ride GmbH & Co. KG",
                        Supplement = "Gebäude 10",
                        Street = "Musterstraße 42",
                        City = "Freiburg",
                        Zip = "79112",
                        CountryCode = "DE",
                    },
                    LineItems = new List<LexQuotationItem>
                    {
                        new LexQuotationItem()
                        {
                            Type = "custom",
                            Name = "test",
                            Quantity = 1,
                            UnitName = "test2",
                            UnitPrice = new LexQuotationUnitPrice()
                            {
                                Currency = "EUR",
                                NetAmount = 5,
                                TaxRatePercentage = 0
                            }
                        }
                    },
                    TotalPrice = new LexQuotationTotalPrice()
                    {
                        Currency = "EUR",
                        TotalNetAmount = 10,
                        TotalGrossAmount = 10,
                        TotalTaxAmount = 10
                    },
                    TaxConditions = new LexQuotationTaxConditions()
                    {
                        TaxType = "net"
                    },
                    ShippingConditions = new LexShippingConditions()
                    {
                        ShippingDate = DateTime.Now,
                        ShippingEndDate = DateTime.Now,
                        ShippingType = "none"
                    },
                    VoucherDate = DateTime.Now,
                };

                LexInvoiceResponse lexInvoiceResponse = await handler.AddInvoiceAsync(invoice, false);

                Assert.That(lexInvoiceResponse != null);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task TestGetInvoicesDraft()
        {
            try
            {
                SecureString token = SecureStringHelper.ConvertToSecureString(tokenString);
                LexOfficeClient handler = new(token);

                List<VoucherListContent> invoicesList = await handler.GetInvoiceListAsync(LexVoucherStatus.draft);
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