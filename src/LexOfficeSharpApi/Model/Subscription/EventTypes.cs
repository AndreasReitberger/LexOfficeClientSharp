namespace AndreasReitberger.API.LexOffice.Model.Subscription
{
    public static class EventTypes
    {
        // Articles
        public const string ArticleCreated = "article.created";
        public const string ArticleChanged = "article.changed";
        public const string ArticleDeleted = "article.deleted";

        // Contacts
        public const string ContactCreated = "contact.created";
        public const string ContactChanged = "contact.changed";
        public const string ContactDeleted = "contact.deleted";

        // Credit Notes
        public const string CreditNoteCreated = "credit-note.created";
        public const string CreditNoteChanged = "credit-note.changed";
        public const string CreditNoteDeleted = "credit-note.deleted";
        public const string CreditNoteStatusChanged = "credit-note.status.changed";

        // Delivery Notes
        public const string DeliveryNoteCreated = "delivery-note.created";
        public const string DeliveryNoteChanged = "delivery-note.changed";
        public const string DeliveryNoteDeleted = "delivery-note.deleted";
        public const string DeliveryNoteStatusChanged = "delivery-note.status.changed";

        // Down Payment Invoices
        public const string DownPaymentInvoiceCreated = "down-payment-invoice.created";
        public const string DownPaymentInvoiceChanged = "down-payment-invoice.changed";
        public const string DownPaymentInvoiceDeleted = "down-payment-invoice.deleted";
        public const string DownPaymentInvoiceStatusChanged = "down-payment-invoice.status.changed";

        // Dunnings
        public const string DunningCreated = "dunning.created";
        public const string DunningChanged = "dunning.changed";
        public const string DunningDeleted = "dunning.deleted";

        // Invoices
        public const string InvoiceCreated = "invoice.created";
        public const string InvoiceChanged = "invoice.changed";
        public const string InvoiceDeleted = "invoice.deleted";
        public const string InvoiceStatusChanged = "invoice.status.changed";

        // Order Confirmations
        public const string OrderConfirmationCreated = "order-confirmation.created";
        public const string OrderConfirmationChanged = "order-confirmation.changed";
        public const string OrderConfirmationDeleted = "order-confirmation.deleted";
        public const string OrderConfirmationStatusChanged = "order-confirmation.status.changed";

        // Payments
        public const string PaymentChanged = "payment.changed";

        // Quotations
        public const string QuotationCreated = "quotation.created";
        public const string QuotationChanged = "quotation.changed";
        public const string QuotationDeleted = "quotation.deleted";
        public const string QuotationStatusChanged = "quotation.status.changed";

        // Recurring Templates
        public const string RecurringTemplateCreated = "recurring-template.created";
        public const string RecurringTemplateChanged = "recurring-template.changed";
        public const string RecurringTemplateDeleted = "recurring-template.deleted";

        // Revoke Token
        public const string TokenRevoked = "token.revoked";

        // Vouchers
        public const string VoucherCreated = "voucher.created";
        public const string VoucherChanged = "voucher.changed";
        public const string VoucherDeleted = "voucher.deleted";
        public const string VoucherStatusChanged = "voucher.status.changed";
    }
}
