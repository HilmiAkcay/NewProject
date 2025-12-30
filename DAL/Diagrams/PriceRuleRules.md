## What PriceRule Is Responsible For (and Nothing Else)

`PriceRule` defines the **base sales price** of a product unit **before promotions and taxes** are applied.

It exists to provide **deterministic, finance-controlled pricing**.

---

### Primary Responsibility

`PriceRule` answers exactly one question:

> **What should the base price of this product unit be for this customer at this point in time?**

---

## What PriceRule MUST Do

### 1. Calculate Base Sales Price
- Convert cost price into a sales price
- Apply margin, fixed price, or permanent base adjustments
- Produce a **single deterministic base price**

---

### 2. Enforce Margin Strategy
- Guarantee agreed profit margins
- Support finance-approved pricing policies
- Prevent accidental selling below cost

Examples:
- Category Wine → 20% margin  
- Wholesale group → fixed base price €6.90  
- Strategic partner → cost + fixed amount  

---

### 3. Support Contractual Pricing
- Handle long-term customer or price group agreements
- Apply fixed prices or fixed margin logic
- Respect validity dates

---

### 4. Resolve Pricing Conflicts (Price-Based)
- Evaluate **all applicable rules**
- Resolve conflicts by **price outcome**, not priority
- Ensure pricing is reproducible and auditable

---

### 5. Be Time-Aware
- Evaluate pricing as-of order date
- Support historical price reconstruction
- Enable dispute resolution and audits

---

### 6. Be Cost-Aware
- Always calculate margins from **real cost**
- Never infer margins from discounts
- Treat cost price as authoritative input

---

## What PriceRule MUST NOT Do

PriceRule MUST NOT:
- Apply temporary promotions or campaigns
- Stack multiple pricing rules
- Track usage or redemption
- Apply marketing logic
- Modify prices after promotions
- Calculate tax or VAT
- Expose itself as a “discount” to customers

---

## Boundaries With Other Systems

| Responsibility | Owned By |
|----------------|----------|
| Cost calculation | Cost / Procurement |
| Base price | PriceRule |
| Discounts & campaigns | Promotion Engine |
| Tax calculation | Tax Engine |

---

## Correct Pricing Flow

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

## Summary

- `PriceRule` defines **what the base price is**
- Promotions define **how the price is reduced**
- Mixing these responsibilities causes audit and margin failures

---

# PriceRule Types (Complete & Controlled)

PriceRules define the **base sales price** of a **ProductUnit**.  
They are **not promotions**, **not temporary**, and **not visible as discounts**.

Only **one final PriceRule result** may apply per ProductUnit.

---

## 1. MARGIN

### Purpose
Define the sales price by applying a margin on top of cost.

**Formula**
BasePrice = CostPrice × (1 + Margin%)

**Use Cases**
- Standard retail pricing
- Category pricing
- Finance margin enforcement

**Allowed Scopes**
- PRODUCT
- PRODUCTVARIANT
- PRODUCTUNIT
- PRICE_GROUP
- GLOBAL

---

## 2. FIXED_PRICE

### Purpose
Force a specific base price regardless of cost.

**Formula**

BasePrice = FixedAmount

**Use Cases**
- Contracts
- Regulated products
- Price matching

**Allowed Scopes**
- PRODUCTUNIT
- PRICE_GROUP
- CUSTOMER

---

## 3. BASE_ADJUSTMENT (Permanent Reduction / Increase)

### Purpose
Apply a permanent percentage adjustment to the calculated base price.


> Replaces the word **DISCOUNT** by design.

**Formula**

BasePrice = CalculatedPrice × (1 ± Adjustment%)


**Use Cases**
- Long-term partner agreements
- Structural pricing alignment

**Allowed Scopes**
- CUSTOMER
- PRICE_GROUP

---

## 4. COST_PLUS_FIXED

### Purpose
Add a fixed amount on top of cost.

**Formula**

BasePrice = CostPrice + FixedAmount

**Use Cases**
- Transparent cost-plus contracts
- Commodity pricing

**Allowed Scopes**
- PRODUCTUNIT
- CUSTOMER

---

## 5. PRICE_FLOOR

### Purpose
Prevent selling below a minimum price.

**Formula**

BasePrice = max(CalculatedPrice, FloorPrice)


### Use Cases
- Legal minimum pricing
- Margin protection
- Anti-dumping controls

**Allowed Scopes**
- PRODUCT
- PRODUCTVARIANT
- PRODUCTUNIT

---

## 6. PRICE_CEILING

### Purpose
Prevent exceeding a maximum price.

**Formula**

BasePrice = min(CalculatedPrice, CeilingPrice)

**Use Cases**
- Regulated pricing
- Public tenders

**Allowed Scopes**
- PRODUCT
- PRODUCTVARIANT
- PRODUCTUNIT

---

## 7. COST_MATCH

### Purpose
Sell at cost (zero margin).


**Formula**

BasePrice = CostPrice
 
**Use Cases**
- Internal sales
- Strategic non-promotional loss leaders

**Allowed Scopes**
- CUSTOMER
- PRICE_GROUP

---

## 8. ROUNDING_OVERRIDE

### Purpose
Override rounding behavior for specific items.

**Formula**

BasePrice = Round(CalculatedPrice, Precision)

 
**Use Cases**
- Psychological pricing
- Cash rounding rules

**Allowed Scopes**
- PRODUCTUNIT

---

## 9. GLOBAL_DEFAULT

### Purpose
Fallback rule when no other rule applies.

**Formula**

BasePrice = CostPrice × (1 + DefaultMargin)
 
**Use Cases**
- Safety fallback
- New product onboarding

**Allowed Scopes**
- GLOBAL only

---

## Forbidden PriceRule Types

These are **explicitly NOT allowed**:

- BUY_X_GET_Y
- TEMPORARY_DISCOUNT
- COUPON
- SEASONAL_PRICE
- LOYALTY_DISCOUNT
- BUNDLE_PRICE
- MIX_AND_MATCH

➡️ These belong to the **Promotion Engine**.

---

# PriceRule Validation Matrix

Violations must be **rejected at write-time**.

## RuleType × ScopeType

| RuleType | PRODUCT | VARIANT | UNIT | PRICE_GROUP | CUSTOMER | GLOBAL |
|--------|--------|--------|------|-------------|----------|--------|
| MARGIN | ✅ | ✅ | ✅ | ✅ | ❌ | ✅ |
| FIXED_PRICE | ❌ | ❌ | ✅ | ✅ | ✅ | ❌ |
| BASE_ADJUSTMENT | ❌ | ❌ | ❌ | ✅ | ✅ | ❌ |
| COST_PLUS_FIXED | ❌ | ❌ | ✅ | ❌ | ✅ | ❌ |
| PRICE_FLOOR | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ |
| PRICE_CEILING | ✅ | ✅ | ✅ | ❌ | ❌ | ❌ |
| COST_MATCH | ❌ | ❌ | ❌ | ✅ | ✅ | ❌ |
| ROUNDING_OVERRIDE | ❌ | ❌ | ✅ | ❌ | ❌ | ❌ |
| GLOBAL_DEFAULT | ❌ | ❌ | ❌ | ❌ | ❌ | ✅ |

---

## Scope Hierarchy (Structural)

Rules may be defined broadly but are **resolved at ProductUnit level**.

GLOBAL
↓
PRODUCT
↓
PRODUCTVARIANT
↓
PRODUCTUNIT


CUSTOMER and PRICE_GROUP are **parallel overlays**, not hierarchy levels.

---

## Rule Stacking

| Scenario | Allowed |
|--------|--------|
| Multiple rules evaluated | ✅ |
| Multiple rules applied | ❌ |
| Promotions after PriceRule | ✅ |

---

## Time Validity

| Case | Result |
|----|-------|
| ValidFrom > ValidTo | ❌ Reject |
| No active rule | ⚠️ Use GLOBAL_DEFAULT |

---

## Value Constraints

- Margin: `0%–100%`
- Base adjustment: `-20% to +20%`
- Fixed price < cost: ❌ unless explicitly allowed
- Floor ≤ Ceiling: mandatory

---

## Abuse Prevention

- CUSTOMER + MARGIN → ❌ forbidden
- CUSTOMER + BASE_ADJUSTMENT → finance approval
- CUSTOMER overriding PRICE_GROUP → ❌ unless explicit
- Visible discounts → ❌ forbidden

---

## Audit & Traceability

Every resolved price MUST log:
- RuleId
- ScopeType
- ScopeId
- CostPrice
- Final BasePrice
- Resolution mode (Highest / Lowest)

---

# PriceRule Resolution Strategy  
## Price-Based (No Priority)

### Core Rule

> All applicable PriceRules are evaluated.  
> **One final base price is selected based on price outcome.**

### Modes
- **Highest price wins** → margin protection
- **Lowest price wins** → customer-favorable

---

## Resolution Steps

1. Collect applicable rules:
   - PRODUCTUNIT
   - PRODUCTVARIANT
   - PRODUCT
   - PRICE_GROUP
   - CUSTOMER
   - GLOBAL

2. Calculate BasePrice per rule  
3. Apply floor / ceiling filters  
4. Discard invalid prices  
5. Sort by price (ASC or DESC)  
6. Select first result  
7. Stop

---

