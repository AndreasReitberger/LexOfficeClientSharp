using AndreasReitberger.API.LexOffice;
using AndreasReitberger.API.LexOffice.Enum;
using Newtonsoft.Json;

namespace LexOfficeSharpApi.Test.NUnit
{
    public class Tests
    {
        private readonly string tokenString = SecretAppSettingReader.ReadSection<SecretAppSetting>("TestSetup").ApiKey ?? "";
        private LexOfficeClient? client;

        #region Setup
        [SetUp]
        public void Setup()
        {
            client = new LexOfficeClient.LexOfficeConnectionBuilder()
                .WithApiKey(tokenString)
                .Build();
        }
        #endregion

        #region JSON
        [Test]
        public void TestJsonSerialization()
        {
            string? json = JsonConvert.SerializeObject(client, Formatting.Indented);
            Assert.That(!string.IsNullOrEmpty(json));

            var client2 = JsonConvert.DeserializeObject<LexOfficeClient>(json);
            Assert.That(client2 is not null);
        }
        #endregion

        #region Builder
        [Test]
        public async Task TestWithBuilder()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");

                List<VoucherListContent> invoicesList = await client.GetInvoiceListAsync(LexVoucherStatus.Paid);
                List<LexDocumentRespone> invoices = await client.GetInvoicesAsync(invoicesList);

                Assert.That(invoices?.Count > 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        #endregion

        #region Invoices
        [Test]
        public async Task TestGetInvoicesOpen()
        {
            try
            {
                LexOfficeClient handler = new(tokenString);

                List<VoucherListContent> invoicesList = await handler.GetInvoiceListAsync(LexVoucherStatus.Open);
                List<LexDocumentRespone> invoices = await handler.GetInvoicesAsync(invoicesList);

                Assert.That(invoices?.Count > 0);
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
                LexOfficeClient handler = new(tokenString);

                // Create a new Invoice object
                LexDocumentRespone invoice = new()
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
                    LineItems =
                    [
                        new LexQuotationItem()
                        {
                            Type = "custom",
                            Name = "Energieriegel Testpaket",
                            Quantity = 1,
                            UnitName = "Stück",
                            UnitPrice = new LexQuotationUnitPrice()
                            {
                                Currency = "EUR",
                                NetAmount = 5,
                                TaxRatePercentage = 0
                            }
                        }
                    ],
                    TotalPrice = new LexQuotationTotalPrice()
                    {
                        Currency = "EUR",
                        TotalNetAmount = 10,
                        TotalGrossAmount = 10,
                        TotalTaxAmount = 10
                    },
                    TaxConditions = new LexQuotationTaxConditions()
                    {
                        TaxType = LexQuotationTaxType.Vatfree,
                    },
                    ShippingConditions = new LexShippingConditions()
                    {
                        ShippingDate = DateTime.Now,
                        ShippingEndDate = DateTime.Now,
                        ShippingType = "none"
                    },
                    PaymentConditions = new LexQuotationPaymentConditions()
                    {
                        PaymentTermLabel = "10 Tage - 3 %, 30 Tage netto",
                        PaymentTermLabelTemplate = "10 Tage - 3 %, 30 Tage netto",
                        PaymentTermDuration = 30,
                        PaymentDiscountConditions = new LexQuotationDiscountCondition() { DiscountPercentage = 3, DiscountRange = 10 }
                    },
                    VoucherDate = DateTime.Now,
                };
                LexResponseDefault? lexInvoiceResponse = await handler.AddInvoiceAsync(invoice, false);
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
                LexOfficeClient handler = new(tokenString);

                List<VoucherListContent> invoicesList = await handler.GetInvoiceListAsync(LexVoucherStatus.Draft);
                List<LexDocumentRespone> invoices = await handler.GetInvoicesAsync(invoicesList);

                Assert.That(invoices?.Count > 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion
          
        #region Payments
        [Test]
        public async Task TestGetPayments()
        {
            try
            {
                LexOfficeClient handler = new(tokenString);

                var invoiceId = Guid.Parse("YOUR_INVOICE_ID");
                LexPayments? payments = await handler.GetPaymentsAsync(invoiceId);

                Assert.That(payments != null);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
      
        #endregion
          
        #region Files

        [Test]
        public async Task TestRenderDocumentAsync()
        {
            try
            {
                LexOfficeClient handler = new(tokenString);

                var invoiceId = Guid.Parse("YOUR_INVOICE_ID");
                LexQuotationFiles files = await handler.RenderDocumentAsync(invoiceId);

                Assert.That(files != null);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task TestGetFileAsync()
        {
            try
            {
                LexOfficeClient handler = new(tokenString);

                var documentId = Guid.Parse("YOUR_FILE_ID");
                byte[] file = await handler.GetFileAsync(documentId);

                Assert.That(file != null);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

       
        #endregion

        #region Countries
        [Test]
        public async Task TestGetCountries()
        {
            try
            {
                LexOfficeClient handler = new(tokenString);
                List<LexCountry> list = await handler.GetCountriesAsync();
                Assert.That(list?.Count > 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        #endregion

        #region Credit Notes
        [Test]
        public async Task TestGetCreditNotes()
        {
            try
            {
                LexOfficeClient handler = new(tokenString);
                List<LexDocumentRespone> list = await handler.GetCreditNotesAsync();
                Assert.That(list?.Count > 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        #endregion

        #region PaymentConditions
        [Test]
        public async Task TestGetPaymentConditions()
        {
            try
            {
                LexOfficeClient handler = new(tokenString);
                List<LexQuotationPaymentConditions> paymentConditions = await handler.GetPaymentConditionsAsync();
                Assert.That(paymentConditions?.Count > 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        #endregion

        #region Quotations
        [Test]
        public async Task TestGetQuotations()
        {
            try
            {
                LexOfficeClient handler = new(tokenString);
                List<VoucherListContent> listContent = await handler.GetQuotationListAsync(LexVoucherStatus.Draft);
                List<LexDocumentRespone> list = await handler.GetQuotationsAsync(listContent);
                Assert.That(list?.Count > 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        #endregion

        #region Contacts
        [Test]
        public async Task TestGetContacts()
        {
            try
            {
                LexOfficeClient handler = new(tokenString);
                List<LexContact> list = await handler.GetContactsAsync(LexContactType.Customer);
                Assert.That(list?.Count > 0);

                list = await handler.GetContactsAsync(LexContactType.Vendor);
                Assert.That(list?.Count > 0);

                Guid id = list.FirstOrDefault().Id;
                var contact = await handler.GetContactAsync(id);
                Assert.That(contact is not null);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        #endregion
    }
}