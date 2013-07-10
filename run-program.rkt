#lang racket

(provide run-program)

(define (run-program str)
  (define-values (proc stdout stdin stderr)
    (subprocess #f #f #f str ""))
  
  (let read-file ([line (read-line stdout)])
    (unless (eof-object? line) 
      (display line)
      (read-file (read-line stdout))))
  
  (close-input-port stdout)
  (close-output-port stdin)
  (close-input-port stderr))