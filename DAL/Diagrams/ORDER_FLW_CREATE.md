```mermaid
flowchart TD
    %% ===============================
    %% ORDER CREATION
    %% ===============================
    A[Start Order] --> B[Add Order Lines]
    B --> C[Recalculate OrderPriceAdjustments]
    C --> D{Add / Remove / Modify Lines?}
    D -->|Yes| B
    D -->|No| E[Review Order / Modify Quantities]
    E --> F[Recalculate OrderPriceAdjustments]
    F --> G{Order Complete?}
    G -->|No| E
    G -->|Yes| H[Finalize Order]
    
    %% ===============================
    %% FULFILLMENT
    %% ===============================
    H --> I[Generate Fulfillment]
    I --> J[Pick Items]
    J --> K[Collect Items]
    K --> L[Package Items]
    L --> M{All Lines Fulfilled?}
    M -->|No| N[Partial Fulfillment Recorded in FulfillmentLine]
    M -->|Yes| O[Fulfillment Completed]
    
    %% ===============================
    %% INVOICE GENERATION
    %% ===============================
    O --> P[Generate Invoice]
    P --> Q[Create InvoiceLines from FulfillmentLines]
    
    %% ===============================
    %% APPLY INVOICE DISCOUNTS
    %% ===============================
    Q --> R[Check for Invoice Discounts]
    R --> S{Discount Source}
    S -->|PROMOTION| T[Map OrderPriceAdjustments → InvoiceDiscount]
    S -->|MANUAL| U[Create Manual InvoiceDiscount]
    T --> V[Allocate Discount Amount to InvoiceLines]
    U --> V
    V --> W[InvoiceDiscountLine Records Created]
    
    %% ===============================
    %% FINALIZE INVOICE
    %% ===============================
    W --> X[Calculate Net Totals]
    X --> Y[Invoice Finalized]
    
    %% ===============================
    %% RETURNS / CANCELLATIONS
    %% ===============================
    Y --> Z{Return / Cancellation?}
    Z -->|Yes| AA[Create Fulfillment of Type RETURN / CANCEL]
    AA --> AB[Update InvoiceLines and Adjust Discounts]
    Z -->|No| AC[Invoice Closed]
