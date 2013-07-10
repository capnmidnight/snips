#lang racket

(define-values (proc stdout stdin stderr)
  (subprocess #f #f #f "C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\csc.exe" ""))

(let read-file ([line (read-line stdout)])
  (unless (eof-object? line) 
    (display line)
    (read-file (read-line stdout))))

(close-input-port stdout)
(close-output-port stdin)
(close-input-port stderr)