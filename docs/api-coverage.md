# Strike API coverage

Audited September 24, 2026 against the [official API reference](https://docs.strike.me/api/) and the operation schemas embedded in that documentation. Version 1.4.0 exposes all 49 documented method/path combinations. Endpoint coverage does not imply every account has permission to use every operation.

## Accounts and balances

| Method and path | Client method |
| --- | --- |
| `GET /v1/accounts/{id}/profile` | `Accounts.GetProfile(Guid)` |
| `GET /v1/accounts/handle/{handle}/profile` | `Accounts.GetProfile(string)` |
| `GET /v1/accounts/limits` | `Accounts.GetLimits()` |
| `GET /v1/balances` | `Balances.GetBalances()` |

Added [account limits](https://docs.strike.me/api/fetch-logged-in-account-limits/) with count and amount limits by period. Balance models now include current, pending, and reserved amounts. Account handles are escaped as URL path segments.

## Currency exchange, deposits, and rates

| Method and path | Client method |
| --- | --- |
| `POST /v1/currency-exchange-quotes` | `CurrencyExchanges.CreateQuote` |
| `GET /v1/currency-exchange-quotes/{quoteId}` | `CurrencyExchanges.GetQuote` |
| `PATCH /v1/currency-exchange-quotes/{quoteId}/execute` | `CurrencyExchanges.ExecuteQuote` |
| `POST /v1/deposits` | `Deposits.Create` |
| `GET /v1/deposits` | `Deposits.GetDeposits` |
| `GET /v1/deposits/{depositId}` | `Deposits.FindDeposit` |
| `POST /v1/deposits/fee` | `Deposits.GetDepositFeeEstimate` |
| `GET /v1/rates/ticker` | `Rates.GetRatesTicker` |

Corrected the deposit request's `feePolicy` field and nullable settlement estimate. See [deposit creation](https://docs.strike.me/api/initiate-deposit/) and [fee estimates](https://docs.strike.me/api/get-deposit-fee-estimate/).

## Invoices

| Method and path | Client method |
| --- | --- |
| `POST /v1/invoices` | `Invoices.IssueInvoice` |
| `POST /v1/invoices/handle/{handle}` | `Invoices.IssueInvoiceFor` |
| `GET /v1/invoices` | `Invoices.GetInvoices` |
| `GET /v1/invoices/{invoiceId}` | `Invoices.FindInvoice` |
| `PATCH /v1/invoices/{invoiceId}/cancel` | `Invoices.CancelInvoice` |
| `POST /v1/invoices/{invoiceId}/quote` | `Invoices.IssueQuote` |

Added [cancellation](https://docs.strike.me/api/cancel-unpaid-invoice/), transaction details with `includeTransactions`, and `$orderby`. OData expressions are URL-encoded, and `descriptionHash` is sent in the quote request body. See [invoice lookup](https://docs.strike.me/api/find-invoice-by-id/) and [invoice quotes](https://docs.strike.me/api/issue-quote-for-invoice/).

## Payment methods and payments

| Method and path | Client method |
| --- | --- |
| `POST /v1/payment-methods/bank` | `PaymentMethods.Create` |
| `GET /v1/payment-methods/bank` | `PaymentMethods.GetPaymentMethods` |
| `GET /v1/payment-methods/bank/{id}` | `PaymentMethods.FindPaymentMethod` |
| `DELETE /v1/payment-methods/bank/{id}` | `PaymentMethods.DeletePaymentMethod` |
| `POST /v1/payment-quotes/lightning` | `PaymentQuotes.CreateLnQuote` |
| `POST /v1/payment-quotes/lightning/lnurl` | `PaymentQuotes.CreateLnurlQuote` |
| `GET /v1/payment-quotes/lightning/lnurl/{lnAddressOrUrl}` | `PaymentQuotes.GetLnurlDetails` |
| `POST /v1/payment-quotes/onchain` | `PaymentQuotes.CreateOnchainQuote` |
| `POST /v1/payment-quotes/onchain/tiers` | `PaymentQuotes.GetOnchainTiers` |
| `PATCH /v1/payment-quotes/{paymentQuoteId}/execute` | `PaymentQuotes.ExecuteQuote` |
| `GET /v1/payments/{paymentId}` | `Payments.FindPayment` |

Added current bank-method currency/BSB fields, nullable account type, payment details, and travel-rule beneficiary data for [Lightning](https://docs.strike.me/api/create-lightning-payment-quote/) and [onchain](https://docs.strike.me/api/create-onchain-payment-quote/) quotes. Bank beneficiary birth dates use `yyyy-MM-dd`; LNURL path values are escaped. Deprecated payment `result` and `delivered` fields remain readable alongside `state` and `completed`.

## Receive requests

| Method and path | Client method |
| --- | --- |
| `POST /v1/receive-requests` | `ReceiveRequests.Create` |
| `GET /v1/receive-requests` | `ReceiveRequests.GetRequests` |
| `GET /v1/receive-requests/{receiveRequestId}` | `ReceiveRequests.FindRequest` |
| `GET /v1/receive-requests/receives` | `ReceiveRequests.GetReceives` |
| `GET /v1/receive-requests/{receiveRequestId}/receives` | `ReceiveRequests.GetReceives(Guid, ...)` |

Corrected optional BTC amount handling and added onchain address reuse metadata. Array filters use repeated, encoded query keys as specified by the operation examples. See [receive request lookup](https://docs.strike.me/api/find-receive-request-by-id/).

## Events and subscriptions

| Method and path | Client method |
| --- | --- |
| `GET /v1/events` | `Events.GetEvents` |
| `GET /v1/events/{eventId}` | `Events.FindEvent` |
| `POST /v1/subscriptions` | `Subscriptions.Create` |
| `GET /v1/subscriptions` | `Subscriptions.GetSubscriptions` |
| `GET /v1/subscriptions/{subscriptionId}` | `Subscriptions.FindSubscription` |
| `PATCH /v1/subscriptions/{subscriptionId}` | `Subscriptions.UpdateSubscription` |
| `DELETE /v1/subscriptions/{subscriptionId}` | `Subscriptions.DeleteSubscription` |

New [events](https://docs.strike.me/api/get-events/) and [subscriptions](https://docs.strike.me/api/get-subscriptions/) clients support webhook v1/v2, unknown version fallback, arbitrary event JSON, nullable delivery status, partial subscription updates, and no-content deletion responses. Event types remain strings so new server-side types do not require a library update.

## Payouts and payout originators

| Method and path | Client method |
| --- | --- |
| `POST /v1/payouts` | `Payouts.Create` |
| `GET /v1/payouts` | `Payouts.GetPayouts` |
| `GET /v1/payouts/{payoutId}` | `Payouts.FindPayout` |
| `PATCH /v1/payouts/{payoutId}/initiate` | `Payouts.InitiatePayout` |
| `POST /v1/payout-originators` | `PayoutOriginators.Create` |
| `GET /v1/payout-originators` | `PayoutOriginators.GetPayoutOriginators` |
| `GET /v1/payout-originators/{originatorId}` | `PayoutOriginators.FindPayoutOriginator` |
| `DELETE /v1/payout-originators/{originatorId}` | `PayoutOriginators.DeletePayoutOriginator` |

New [payout](https://docs.strike.me/api/get-payouts/) and [originator](https://docs.strike.me/api/get-payout-originators/) clients include individual/company request models, date-only birth dates, state enums, optional fees, references, and transaction IDs.

## Compatibility and validation

Existing public signatures and enum numeric values are retained. `Currency.Aud` is appended after the existing values. The library still targets .NET Core 3.1, .NET 6, .NET 8, and .NET 10.

Existing paginated types keep `int Count` and add `long TotalCount`, mapped to the API's `count`. Reading legacy `Count` above `Int32.MaxValue` throws `OverflowException`; read `TotalCount` instead. New paginated types use `long Count`. All expose `IsCountUnknown`.

Nullable API values use new properties where changing an existing CLR type would break consumers: `SettlementTime` for a deposit fee estimate, `RequestedBtcAmount` for receive requests, and `OptionalAccountType` for bank methods. Existing properties remain compatibility projections. `DepositReq.Fee` remains available and maps to `FeePolicy`.

`Accounts.GetProfile()` (`GET /v1/accounts/profile`) and `Invoices.FindQuote` (`GET /v1/quotes/{id}`) remain available for compatibility. Neither operation appears in the current published reference, so their current server behavior is not guaranteed by this audit.

Requests now follow the documented camelCase field names. Idempotency keys are sent only as headers. Error handling preserves HTTP status for missing/malformed error bodies and exposes structured error details on `StrikeApiException.Error`, plus development diagnostics on `StrikeError.Debug`.

Offline contract tests verify request paths, verbs, headers, serialization, pagination, nullable fields, enums, and response handling. Live tests make GET requests only; a handler blocks writes before transmission. Account limits could not be verified live because the configured key receives 403 for the required `partner.account.profile.read-own` scope. Money-moving endpoints are tested only with offline handlers.

Release validation passed with zero-warning Debug and Release builds, 238 offline tests, and 22 live read-only checks across .NET 8 and .NET 10. APICompat found no public API breaks against the published 1.3.2 assemblies on all four library targets. Packaged consumer smoke tests passed on .NET Core 3.1 and .NET 5 through 10.
