using AndreasReitberger.API.LexOffice;
using AndreasReitberger.API.LexOffice.Enum;
using Newtonsoft.Json;
using System.Diagnostics;

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
                .WithWebAddress()
                .WithApiKey(tokenString)
                .WithTimeout(100 * 1000)
                // A client can make up to 2 requests per second to the lexoffice API.
                //.WithRateLimiter(true, tokenLimit: 2, tokensPerPeriod: 2, replenishmentPeriod: 1.5)
                .Build();
            client.Error += (sender, args) =>
            {
                if (args is UnhandledExceptionEventArgs a)
                {
                    Debug.WriteLine($"Error: {a.ExceptionObject}");
                    //Assert.Fail($"Error: {args?.ToString()}");
                }
            };
            client.RestApiError += (sender, args) =>
            {
                Debug.WriteLine($"REST-Error: {args?.ToString()}");
                //Assert.Fail($"REST-Error: {args?.ToString()}");
            };
        }
        #endregion

        #region JSON
        [Test]
        public void TestJsonSerialization()
        {
            try
            {
                string? json = JsonConvert.SerializeObject(client, Formatting.Indented, settings: LexOfficeClient.DefaultNewtonsoftJsonSerializerSettings);
                Assert.That(!string.IsNullOrEmpty(json));

                LexOfficeClient? client2 = JsonConvert.DeserializeObject<LexOfficeClient>(json, settings: LexOfficeClient.DefaultNewtonsoftJsonSerializerSettings);
                Assert.That(client2, Is.Not.EqualTo(null));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
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
                List<LexDocumentResponse> invoices = await client.GetInvoicesAsync(invoicesList);

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
                List<LexDocumentResponse> invoices = await handler.GetInvoicesAsync(invoicesList);

                Assert.That(invoices?.Count, Is.GreaterThan(0));
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
                if (client is null) throw new NullReferenceException($"The client was null!");
                // Create a new Invoice object
                LexDocumentResponse invoice = new()
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
                LexResponseDefault? lexInvoiceResponse = await client.AddInvoiceAsync(invoice, false);
                Assert.That(lexInvoiceResponse, Is.Not.EqualTo(null));
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
                if (client is null) throw new NullReferenceException($"The client was null!");
                List<VoucherListContent> invoicesList = await client.GetInvoiceListAsync(LexVoucherStatus.Draft);
                List<LexDocumentResponse> invoices = await client.GetInvoicesAsync(invoicesList);

                Assert.That(invoices?.Count, Is.GreaterThan(0));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task TestCreateCreditNoteDraft()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                
                List<VoucherListContent> invoicesList = await client.GetInvoiceListAsync(LexVoucherStatus.Open);
                List<LexDocumentResponse> invoices = await client.GetInvoicesAsync(invoicesList);
                var invoice = invoices.FirstOrDefault() ?? throw new NullReferenceException("No invoice found!");
                
                string voucherNumber = invoice.VoucherNumber;
                invoice.Title = "Rechnungskorrektur";
                invoice.Introduction = $"Rechnungskorrektur zur Rechnung {voucherNumber}";

                invoice.LineItems.Add(
                    new LexQuotationItem()
                    {
                        Type = "custom",
                        Name = "Energieriegel Testpaket",
                        Quantity = 0.1m,
                        UnitName = "Stück",
                        UnitPrice = new LexQuotationUnitPrice()
                        {
                            Currency = "EUR",
                            NetAmount = -150,
                            TaxRatePercentage = 0
                        }
                    }
                );

                LexResponseDefault? rs = await client.AddCreditNoteAsync(invoice, true);
                Assert.That(rs, Is.Not.EqualTo(null));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task TestAddEventSubscription()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                // Cleanup available event subscriptions
                List<LexResponseDefault>? allSubscriptions = await client.GetAllEventSubscriptionsAsync() ?? throw new NullReferenceException("No subscriptions found!");
                foreach (LexResponseDefault subscriptions in allSubscriptions)
                {
                    await client.DeleteEventSubscriptionAsync(subscriptions.SubscriptionId);
                }

                // Create event subscription
                LexResponseDefault? newSubscription = await client.AddEventSubscriptionAsync(new LexResponseDefault
                {
                    EventType = EventTypes.PaymentChanged,
                    CallbackUrl = "https://webhook.site/11dac08c-7a64-4467-aae9-8ec5dd1f3338"
                });

                LexResponseDefault? subscription = await client.GetEventSubscriptionAsync(newSubscription?.Id);
                using (Assert.EnterMultipleScope())
                {
                    Assert.That(newSubscription, Is.Not.EqualTo(null));
                    Assert.That(newSubscription?.Id, Is.Not.EqualTo(subscription?.Id));
                    Assert.That(newSubscription?.EventType, Is.Not.EqualTo(subscription?.EventType));
                }
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
                if (client is null) throw new NullReferenceException($"The client was null!");
                
                List<VoucherListContent> availableInvoices = await client.GetInvoiceListAsync(LexVoucherStatus.Paid, size: 1, pages: 1);
                Guid invoiceId = availableInvoices.First().Id; // Guid.Parse("YOUR_INVOICE_ID");
                Assert.That(invoiceId, Is.Not.EqualTo(Guid.Empty));

                LexPayments? payments = await client.GetPaymentsAsync(invoiceId);
                Assert.That(payments, Is.Not.EqualTo(null));
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
                if (client is null) throw new NullReferenceException($"The client was null!");
                List<VoucherListContent> availableInvoices = await client.GetInvoiceListAsync(LexVoucherStatus.Paid, size: 1, pages: 1);
                Guid invoiceId = availableInvoices.First().Id; // Guid.Parse("YOUR_INVOICE_ID");
                Assert.That(invoiceId, Is.Not.EqualTo(Guid.Empty));

                LexDocumentResponse? invoice = await client.GetInvoiceAsync(availableInvoices.First().Id);
                LexQuotationFiles? files = await client.RenderDocumentAsync(invoiceId);

                Assert.That(files, Is.Not.EqualTo(null));
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
                if (client is null) throw new NullReferenceException($"The client was null!");

                List<VoucherListContent> availableInvoices = await client.GetInvoiceListAsync(LexVoucherStatus.Paid, size: 1, pages: 1);
                Guid invoiceId = availableInvoices.First().Id; // Guid.Parse("YOUR_INVOICE_ID");
                Assert.That(invoiceId, Is.Not.EqualTo(Guid.Empty));

                LexDocumentResponse? invoice = await client.GetInvoiceAsync(availableInvoices.First().Id);
                LexQuotationFiles? files = await client.RenderDocumentAsync(invoiceId);
                Assert.That(files, Is.Not.EqualTo(null));

                Guid documentId = files.DocumentFileId;// Guid.Parse("YOUR_FILE_ID");
                byte[]? file = await client.GetFileAsync(documentId);
                Assert.That(file, Is.Not.EqualTo(null));

                await File.WriteAllBytesAsync($"invoice.pdf", file);
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
                if (client is null) throw new NullReferenceException($"The client was null!");
                List<LexCountry> list = await client.GetCountriesAsync();
                Assert.That(list?.Count, Is.GreaterThan(0));
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
                if (client is null) throw new NullReferenceException($"The client was null!");
                List<LexDocumentResponse> list = await client.GetCreditNotesAsync();
                Assert.That(list?.Count, Is.GreaterThan(0));
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
                if (client is null) throw new NullReferenceException($"The client was null!");
                List<LexQuotationPaymentConditions> paymentConditions = await client.GetPaymentConditionsAsync();
                Assert.That(paymentConditions?.Count, Is.GreaterThan(0));
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
                if (client is null) throw new NullReferenceException($"The client was null!");
                List<VoucherListContent> listContent = await client.GetQuotationListAsync(LexVoucherStatus.Accepted);
                List<LexDocumentResponse> list = await client.GetQuotationsAsync(listContent);
                Assert.That(list?.Count, Is.GreaterThan(0));
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
                if (client is null) throw new NullReferenceException($"The client was null!");
                List<LexContact> list = await client.GetContactsAsync(LexContactType.Customer, size: 100, pages: 2);
                Assert.That(list?.Count, Is.GreaterThan(0));

                await Task.Delay(500);

                list = await client.GetContactsAsync(LexContactType.Vendor, size: 100, pages: 2);
                Assert.That(list?.Count, Is.GreaterThan(0));

                Guid id = list?.FirstOrDefault()?.Id ?? Guid.Empty;
                var contact = await client.GetContactAsync(id);
                Assert.That(contact, Is.Not.EqualTo(null));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        #endregion

        #region RateLimiter
        [Test]
        public async Task TestRateLimiterOnGetContacts()
        {
            try
            {
                if (client is null) throw new NullReferenceException($"The client was null!");
                List<LexContact> list = await client.GetContactsAsync(LexContactType.Customer, size: 100, pages: -1);
                Assert.That(list?.Count, Is.GreaterThan(0));

                await Task.Delay(500);

                list = await client.GetContactsAsync(LexContactType.Vendor, size: 100, pages: 2);
                Assert.That(list?.Count, Is.GreaterThan(0));

                Guid id = list?.FirstOrDefault()?.Id ?? Guid.Empty;
                var contact = await client.GetContactAsync(id);
                Assert.That(contact, Is.Not.EqualTo(null));

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion

        #region Cleanup
        [TearDown]
        public void BaseTearDown()
        {
            client?.Dispose();
        }
        #endregion
    }
}