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

# PriceRule Types (Complete & Controlled)

PriceRules define the **base sales price** of a product unit.
They are **not promotions**, **not temporary campaigns**, and **not visible as discounts**.

Only **one PriceRule** may apply per ProductUnit.

---

## 1. MARGIN

### Purpose
Define the sales price by applying a margin on top of cost.

### Formula
BasePrice = CostPrice × (1 + Margin%)


### Use Cases
- Standard retail pricing
- Category-based pricing
- Finance-controlled profit strategy

### Allowed Scopes
- PRODUCT
- PRODUCTVARIANT
- PRODUCTUNIT
- PRICE_GROUP

### Forbidden
- CUSTOMER (unless explicitly approved)

---

## 2. FIXED_PRICE

### Purpose
Force a specific base price regardless of cost.

### Formula
BasePrice = FixedAmount

### Use Cases
- Contract pricing
- Regulated products
- Price-matched items

### Allowed Scopes
- PRODUCTUNIT
- CUSTOMER
- PRICE_GROUP

### Forbidden
- PRODUCT (too broad)

---

## 3. BASE_ADJUSTMENT (Permanent Reduction / Increase)

### Purpose
Apply a permanent percentage adjustment to the calculated base price.

> This replaces the word **DISCOUNT** to avoid misuse.

### Formula
BasePrice = CalculatedPrice × (1 ± Adjustment%)


### Use Cases
- Long-term customer agreements
- Permanent partner pricing
- Strategic pricing alignment

### Allowed Scopes
- CUSTOMER
- PRICE_GROUP

### Forbidden
- PRODUCT
- PRODUCTVARIANT
- PRODUCTUNIT

---

## 4. COST_PLUS_FIXED

### Purpose
Add a fixed amount on top of cost.

### Formula
BasePrice = CostPrice + FixedAmount


### Use Cases
- Low-margin commodities
- Transparent cost-plus contracts

### Allowed Scopes
- PRODUCTUNIT
- CUSTOMER

---

## 5. PRICE_FLOOR

### Purpose
Prevent selling below a minimum price.

### Formula
BasePrice = max(CalculatedPrice, FloorPrice)


### Use Cases
- Legal minimum pricing
- Margin protection
- Anti-dumping controls

### Allowed Scopes
- PRODUCT
- PRODUCTVARIANT
- PRODUCTUNIT

---

## 6. PRICE_CEILING

### Purpose
Prevent exceeding a maximum price.

### Formula
BasePrice = min(CalculatedPrice, CeilingPrice)


### Use Cases
- Regulated pricing
- Price promises
- Public sector contracts

### Allowed Scopes
- PRODUCT
- PRODUCTVARIANT
- PRODUCTUNIT

---

## 7. COST_MATCH

### Purpose
Sell at cost (zero margin).

### Formula
BasePrice = CostPrice


### Use Cases
- Internal transfers
- Employee sales
- Strategic loss leaders (non-promotional)

### Allowed Scopes
- CUSTOMER
- PRICE_GROUP

---

## 8. ROUNDING_OVERRIDE

### Purpose
Override rounding behavior for specific items.

### Formula
BasePrice = Round(CalculatedPrice, RulePrecision)


### Use Cases
- Psychological pricing
- Cash payment constraints

### Allowed Scopes
- PRODUCTUNIT

---

## 9. GLOBAL_DEFAULT

### Purpose
Fallback rule when no other rule applies.

### Formula
BasePrice = CostPrice × (1 + DefaultMargin)


### Use Cases
- System safety net
- New product onboarding

### Allowed Scopes
- GLOBAL only

---

## Forbidden PriceRule Types (By Design)

These are **explicitly NOT allowed** as PriceRules:

- BUY_X_GET_Y
- TEMPORARY_DISCOUNT
- SEASONAL_PRICE
- COUPON
- LOYALTY_DISCOUNT
- BUNDLE_PRICE
- MIX_AND_MATCH

> These belong to the **Promotion Engine**, not PriceRule.

---

## Final Rules (Non-Negotiable)

- One PriceRule per ProductUnit
- No stacking
- No visibility as discount
- Promotions apply **after** PriceRule
- Finance owns PriceRule definitions





