## What PriceRule Is Responsible For (and Nothing Else)

`PriceRule` defines the **base sales price** of a product **before promotions and taxes** are applied.

It exists to provide **deterministic, finance-controlled pricing**.

---

### Primary Responsibility

`PriceRule` answers exactly one question:

> **What should the base price of this product be for this customer at this point in time?**

---

### What PriceRule MUST Do

#### 1. Calculate Base Sales Price
- Convert cost price into sales price
- Apply margin, fixed price, or permanent discount rules
- Produce a deterministic base price


---

#### 2. Enforce Margin Strategy
- Guarantee agreed profit margins
- Support finance-approved pricing policies
- Prevent accidental selling below cost

Examples:
- Gold customers → 5% margin
- Category Wine → 20% margin
- Wholesale contract → fixed price €6.90

---

#### 3. Support Contractual Pricing
- Handle long-term customer or group agreements
- Apply fixed prices or fixed margins
- Respect contract validity dates

---

#### 4. Resolve Pricing Conflicts
- Select exactly one applicable rule
- Use priority to resolve overlaps
- Ensure pricing is predictable and reproducible

---

#### 5. Be Time-Aware
- Evaluate pricing as-of order date
- Support historical price reconstruction
- Enable accurate auditing and dispute resolution

---

#### 6. Be Cost-Aware
- Always calculate margin from real cost price
- Never infer margin from discounts
- Treat cost price as authoritative input

---

### What PriceRule MUST NOT Do

PriceRule MUST NOT:
- Apply temporary promotions or campaigns
- Stack multiple discounts or margins
- Track usage or redemption
- Apply marketing logic
- Modify prices after promotions
- Calculate tax or VAT

---

### Boundaries With Other Systems

| Responsibility | Owned By |
|----------------|----------|
| Cost calculation | Cost / Procurement |
| Base price | PriceRule |
| Discounts & campaigns | Promotion Engine |
| Tax calculation | Tax Engine |

---

### Correct Pricing Flow

Cost Price
↓
PriceRule (Base Price)
↓
Promotion Engine
↓
Tax Engine
↓
Final Payable Price


---

### Summary

- `PriceRule` defines **what the price should be**
- Promotions define **how the price is reduced**
- Mixing these responsibilities leads to inconsistent pricing and audit failures




