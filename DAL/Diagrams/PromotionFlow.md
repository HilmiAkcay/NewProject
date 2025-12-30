flowchart LR
    A[Product Cost<br/>CostPrice] --> B[Pricing Rules<br/>Margin / Contract Rules]
    B --> C[Base Sales Price<br/>Cost + Margin]
    C --> D[Promotion Engine<br/>Discounts / Free Items]
    D --> E[Final Sales Price]
    E --> F[Tax Engine<br/>VAT / Tax Rules]
    F --> G[Payable Price]

    %% Styling and Annotations
    classDef pricing fill:#e3f2fd,stroke:#1e88e5,stroke-width:1px
    classDef promo fill:#fff3e0,stroke:#fb8c00,stroke-width:1px
    classDef tax fill:#e8f5e9,stroke:#43a047,stroke-width:1px

    class B pricing
    class D promo
    class F tax


# COMPLETE PROMOTION SCENARIO MATRIX

This document lists **all business scenarios** supported by the promotion data model.

---

## 1. Price Reduction Scenarios

### 1.1 Simple Percentage Discount
**Example:** 10% off all wines  
- **Condition:** `CATEGORY = Wine`  
- **Action:** `PERCENT_DISCOUNT = 10`

---

### 1.2 Fixed Amount Discount
**Example:** €5 off when spending €50  
- **Condition:** `MIN_AMOUNT >= 50`  
- **Action:** `FIXED_DISCOUNT = 5`

---

### 1.3 Product-Specific Discount
**Example:** €2 off Product A  
- **Condition:** `PRODUCT = ProductA`  
- **Action:** `FIXED_DISCOUNT = 2`

---

### 1.4 Capped Percentage Discount
**Example:** 20% off, max €15  
- **Action:**  
  - `PERCENT_DISCOUNT = 20`  
  - `MaxAmount = 15`

---

## 2. Fixed / Contract Pricing (B2B)

### 2.1 Customer-Specific Contract Price
**Example:** Customer X buys Product Y for €6.90  
- **Type:** `B2B_AGREEMENT`  
- **Condition:** `CUSTOMER = X`, `PRODUCT = Y`  
- **Action:** `FIXED_PRICE = 6.90`

---

### 2.2 Price Group Agreement
**Example:** Horeca pricing  
- **Condition:** `PRICE_GROUP = Horeca`  
- **Action:** `FIXED_PRICE`

---

### 2.3 Time-Bound Contract Pricing
**Example:** Seasonal wholesale pricing  
- **Condition:** `StartDate / EndDate`  
- **Action:** `FIXED_PRICE`

---

## 3. Quantity-Based Promotions

### 3.1 Buy X Get Discount
**Example:** Buy 6 items → 10% off  
- **Condition:** `MIN_QTY >= 6`  
- **Action:** `PERCENT_DISCOUNT = 10`

---

### 3.2 Tiered Quantity Discounts
**Example:** Buy 3 → €1 off, Buy 6 → €3 off  
- **Implementation:** Multiple promotions  
- **Priority:** Higher quantity = higher priority

---

### 3.3 Mix & Match Quantity Discount
**Example:** Any 3 beers → €2 off  
- **Condition:** `CATEGORY = Beer`  
- **Condition:** `MIN_QTY >= 3`  
- **Action:** `FIXED_DISCOUNT = 2`

---

## 4. Free Item / Bonus Promotions

### 4.1 Buy X Get Y Free
**Example:** Buy 12 bottles → free opener  
- **Condition:** `PRODUCT = Wine`  
- **Condition:** `MIN_QTY >= 12`  
- **Action:** `FREE_ITEM = Opener`

---

### 4.2 Spend-Based Free Item
**Example:** Spend €100 on spirits → free glass  
- **Condition:** `CATEGORY = Spirits`  
- **Condition:** `MIN_AMOUNT >= 100`  
- **Action:** `FREE_ITEM = Glass`

---

## 5. Customer & Loyalty Promotions

### 5.1 Customer-Specific Discount
**Example:** Customer A always gets 5%  
- **Condition:** `CUSTOMER = A`  
- **Action:** `PERCENT_DISCOUNT = 5`

---

### 5.2 Loyalty Tier Discount
**Example:** Gold customers get 10%  
- **Condition:** `PRICE_GROUP = Gold`  
- **Action:** `PERCENT_DISCOUNT = 10`

---

### 5.3 First Order Promotion
**Example:** New customer 10% off  
- **Condition:** Evaluated by engine logic  
- **Action:** `PERCENT_DISCOUNT = 10`

---

## 6. Brand & Category Campaigns

### 6.1 Brand-Based Promotion
**Example:** All Heineken products −15%  
- **Condition:** `BRAND = Heineken`  
- **Action:** `PERCENT_DISCOUNT = 15`

---

### 6.2 Category Campaign
**Example:** All whisky €3 off  
- **Condition:** `CATEGORY = Whisky`  
- **Action:** `FIXED_DISCOUNT = 3`

---

## 7. Stacking & Priority Scenarios

### 7.1 Non-Stackable Override
**Example:** Contract price overrides all other promotions  
- **Priority:** High  
- **IsStackable:** `false`

---

### 7.2 Stackable Promotions
**Example:** Weekend 5% + Loyalty 5%  
- **IsStackable:** `true`  
- **Applied in priority order**

---

### 7.3 Promotion Exclusion
**Example:** Applies only if no other promotion is applied  
- **IsStackable:** `false`  
- **Priority:** Lower than contracts

---

## 8. Channel Usage Scenarios

### 8.1 POS
- Automatic application  
- High performance required  
- Usage logged

---

### 8.2 Web Shop
- Same engine as POS  
- Same business rules  
- Same audit trail

---

### 8.3 Backoffice / Manual Orders
- Manual override allowed  
- Promotion usage still logged

---

## 9. Audit & Control Scenarios

### 9.1 Promotion Cost Analysis
**Question:** How much did Promotion X cost?  
- **Source:** `PromoUsageLog`

---

### 9.2 Usage Limits
**Example:** Once per customer  
- **Enforced by:** Engine logic + usage log

---

### 9.3 Abuse Detection
- Detect repeated usage patterns  
- Identify promotion misuse

---

## 10. Out-of-Scope (Not Promotions)

The following are intentionally **not handled** by this model:

- Supplier rebates  
- Retroactive price changes  
- Manual price overrides  
- Stock transfers  
- Financial settlements

---

## Final Notes

This promotion structure supports:
- Retail  
- Wholesale  
- POS  
- Web  
- Loyalty programs  
- Contract pricing  

If a scenario does not fit here, it likely **does not belong in a promotion engine**.
