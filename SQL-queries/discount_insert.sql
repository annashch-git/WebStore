INSERT INTO discount_codes (
    "code",                -- Discount code string, e.g., 'SUMMER2025'
    "description",         -- Human-readable description of the discount
    "DiscountType",        -- Enum value: 0 = Percentage, 1 = Flat
    "DiscountValue",       -- Numeric value of the discount (e.g., 10 for 10%)
    "ExpirationDate",      -- Optional expiration date for the discount
    "MaxUsage",            -- Optional maximum times the code can be used
    "TimesUsed",           -- How many times the code has been used so far
    "discount_amount",     -- Actual discount amount to apply
    "is_percentage"        -- Boolean flag: true if percentage, false if flat amount
)
VALUES (
    'SUMMER2025',           -- Code name for the discount
    '10% off entire order', -- Description
    0,                      -- DiscountType enum value: 0 = Percentage
    10,                     -- 10% discount
    '2025-09-01',           -- Expires on September 1, 2025
    100,                    -- Can be used up to 100 times
    0,                      -- Hasn't been used yet
    10.00,                  -- Discount amount (used in logic or for display)
    true                    -- It's a percentage-based discount
);
