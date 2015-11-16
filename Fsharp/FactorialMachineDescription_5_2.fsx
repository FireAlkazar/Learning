let controller =
    [
        "assign (reg product) (const 1)"
        "assign (reg counter) (const 1)"
        "test-counter-greater"
        "test (op >) (reg counter) N"
        "branch label factorial-done"
        "assign t_p (op *) (reg product) (reg counter)"
        "assign product (reg t_p)"
        "assign t_c (op +) (reg counter) (const 1)"
        "assign counter (reg t_c)"
        "goto label test-counter-greater"
        "factorial-done"
    ]
