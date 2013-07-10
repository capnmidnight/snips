#lang racket/gui

(define frm (new frame% 
                 [label "Test"]
                 [width 600]
                 [height 300]))

(send frm show #t)

(define 2d-grid%
  (class vertical-panel%
    (super-new [style '(hscroll vscroll border)])
    
    (init-field columns)
    (unless (and (list? columns) (andmap symbol? columns))
      (error "Expected list of symbols for init-field columns"))
    
    (define body-row (new horizontal-panel%
                          [horiz-margin 0]
                          [vert-margin 0]
                          [parent this]))
    
    (define original-data #f)
    
    ;; ======================
    (define/public (get-columns)
      columns)
    
    ;; ======================
    (define/public (get-data)
      original-data)
    
    ;; ============================
    (define/public (get-row-data i)
      (list-ref original-data i))
    
    ;; ===============================
    (define/public (get-column-data i)
      (for/list ([row original-data])
        (list-ref row i)))
    
    ;; ===========================================
    (define (fill-column col-panel i header-width)
      (for ([row original-data])
        (new text-field% 
             [label #f]
             [init-value (format "~a" (list-ref row i))]
             [horiz-margin 0]
             [vert-margin 0]
             [min-width header-width]
             [parent col-panel])))
    
    ;; ===========================
    (define (make-column column i)
      (let* ([col-panel (new vertical-panel%
                             [min-height 30]
                             [horiz-margin 0]
                             [vert-margin 0]
                             [stretchable-height #f]
                             [parent body-row])]
             [header (new button% 
                          [label (format "~a~a" column (if (equal? (car cur-sort) i)
                                                           (if (cdr cur-sort)
                                                               " (asc)"
                                                               " (desc)")
                                                           ""))]
                          [horiz-margin 0]
                          [vert-margin 0]
                          [stretchable-height #t]
                          [callback (lambda (btn evt) (sort-column i))]
                          [parent col-panel])]
             [header-width (send header get-width)])
        (fill-column col-panel i header-width)))
    
    ;; ====================
    (define (build-columns)
      (for ([child (send body-row get-children)])
        (send body-row delete-child child))
      
      (for ([column columns]
            [i (in-range (length columns))])
        (make-column column i)))
    
    ;; ===========================
    (define/public (set-data data)
      (unless (and (list? data) 
                   (andmap (lambda (row) 
                             (equal? 
                              (length row) 
                              (length columns))) 
                           data))
        (error (format "Excpected list of R lists of C elements, where C is the number of columns: ~a
Received instead: ~a" (length columns) (map length data))))
      
      (set! original-data data)
      
      (build-columns))
    
    (define cur-sort '(() . ()))
    
    ;; ===========================
    (define/public (sort-column i)
      (set! original-data (sort original-data 
                                (if (and (equal? (car cur-sort) i) (cdr cur-sort)) > <)
                                #:key (lambda (row) (list-ref row i))))
      (set! cur-sort (cons i (not (and (equal? (car cur-sort) i) (cdr cur-sort)))))
      (build-columns))
    
    ))

(define data (for/list ([rows (in-range 10)])
               (for/list ([cols (in-range 10)])
                 (random 100))))

(define grd (new 2d-grid% 
                 [parent frm]
                 [columns '(a b c d e f g h i j)]))

(send grd set-data data)

